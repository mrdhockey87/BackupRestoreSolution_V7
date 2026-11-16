// BackupInfo_Implementation.cpp - Get backup information and list contents
#include "BackupEngine.h"
#include "WimMountManager.h"
#include <Windows.h>
#include <Rpc.h>
#include <string>
#include <filesystem>
#include <sstream>
#include <fstream>
#include <vector>
#include <algorithm>

#pragma comment(lib, "Rpcrt4.lib")

namespace fs = std::filesystem;
extern void SetLastErrorMessage(const std::wstring& error);

namespace {
    bool IsArchiveFilePath(const std::wstring& path) {
        std::wstring extension = fs::path(path).extension().wstring();
        return _wcsicmp(extension.c_str(), L".ssb") == 0 ||
               _wcsicmp(extension.c_str(), L".wim") == 0;
    }

    // Context for the enumerate-without-extract approach
    struct FileEnumContext {
        std::vector<std::wstring> files;
        std::wstring applyDirPrefix; // applyDir + L"\\" so we can strip it from every path
    };

    // WIM message callback that collects relative file paths but skips all extraction
    static DWORD WINAPI FileEnumCallback(DWORD msgId, WPARAM wParam, LPARAM lParam, PVOID pvContext) {
        if (msgId == WIM_MSG_PROCESS && wParam && lParam) {
            const wchar_t* filePath = reinterpret_cast<const wchar_t*>(wParam);
            BOOL* pbInclude = reinterpret_cast<BOOL*>(lParam);

            // Always skip extraction - we only want the path
            *pbInclude = FALSE;

            if (filePath && *filePath) {
                FileEnumContext* ctx = reinterpret_cast<FileEnumContext*>(pvContext);
                if (ctx) {
                    std::wstring relPath(filePath);

                    // Strip the apply-dir prefix so we store a relative path like "Users\file.txt"
                    if (!ctx->applyDirPrefix.empty() &&
                        relPath.size() > ctx->applyDirPrefix.size() &&
                        _wcsnicmp(relPath.c_str(), ctx->applyDirPrefix.c_str(), ctx->applyDirPrefix.size()) == 0) {
                        relPath = relPath.substr(ctx->applyDirPrefix.size());
                    }

                    if (!relPath.empty()) {
                        ctx->files.push_back(relPath);
                    }
                }
            }
        }
        return WIM_MSG_SUCCESS;
    }

    // Recursively remove a directory tree using Win32 (no filesystem exceptions on special paths)
    void RemoveDirTree(const std::wstring& dir) {
        if (dir.empty()) {
            return;
        }

        WIN32_FIND_DATAW fd = {};
        std::wstring pattern = dir + L"\\*";
        HANDLE hFind = FindFirstFileW(pattern.c_str(), &fd);
        if (hFind != INVALID_HANDLE_VALUE) {
            do {
                if (wcscmp(fd.cFileName, L".") == 0 || wcscmp(fd.cFileName, L"..") == 0) {
                    continue;
                }
                std::wstring child = dir + L"\\" + fd.cFileName;
                if (fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
                    RemoveDirTree(child);
                } else {
                    DeleteFileW(child.c_str());
                }
            } while (FindNextFileW(hFind, &fd));
            FindClose(hFind);
        }
        RemoveDirectoryW(dir.c_str());
    }

    // Create a unique temp subdirectory for the dry-run apply target
    std::wstring CreateUniqueTempDir() {
        wchar_t tempBase[MAX_PATH] = {};
        if (GetTempPathW(MAX_PATH, tempBase) == 0) {
            return L"";
        }

        UUID uuid = {};
        UuidCreate(&uuid);
        RPC_WSTR uuidStr = nullptr;
        if (UuidToStringW(&uuid, &uuidStr) != RPC_S_OK || !uuidStr) {
            return L"";
        }

        std::wstring dir = std::wstring(tempBase) + L"SSBList_" + reinterpret_cast<wchar_t*>(uuidStr);
        RpcStringFreeW(&uuidStr);

        CreateDirectoryW(dir.c_str(), nullptr);
        return dir;
    }

    // Recursively enumerate files from a directory tree using Win32 (no filesystem exceptions)
    void EnumerateFilesWin32(const std::wstring& dir, const std::wstring& relBase, std::vector<std::wstring>& outFiles) {
        std::wstring pattern = dir + L"\\*";
        WIN32_FIND_DATAW fd = {};
        HANDLE hFind = FindFirstFileW(pattern.c_str(), &fd);
        if (hFind == INVALID_HANDLE_VALUE) {
            return;
        }

        do {
            if (wcscmp(fd.cFileName, L".") == 0 || wcscmp(fd.cFileName, L"..") == 0) {
                continue;
            }

            std::wstring relPath = relBase.empty() ? fd.cFileName : relBase + L"\\" + fd.cFileName;

            if (fd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
                EnumerateFilesWin32(dir + L"\\" + fd.cFileName, relPath, outFiles);
            } else {
                // Skip metadata files
                if (wcscmp(fd.cFileName, L"backup_metadata.dat") != 0 &&
                    wcscmp(fd.cFileName, L"backup_info.txt") != 0) {
                    outFiles.push_back(relPath);
                }
            }
        } while (FindNextFileW(hFind, &fd));

        FindClose(hFind);
    }

    // List files from a .ssb archive: apply fully to temp dir, enumerate, clean up
    bool TryListArchiveContents(const std::wstring& backupPath, int imageIndex, std::vector<std::wstring>& outFiles) {
        HANDLE hWim = WIMCreateFile(
            backupPath.c_str(),
            WIM_GENERIC_READ,
            WIM_OPEN_EXISTING,
            0,
            0,
            nullptr);

        if (!hWim || hWim == INVALID_HANDLE_VALUE) {
            SetLastErrorMessage(L"Failed to open SSB archive for content listing");
            return false;
        }

        wchar_t tempPath[MAX_PATH] = {};
        if (GetTempPathW(MAX_PATH, tempPath) > 0) {
            WIMSetTemporaryPath(hWim, tempPath);
        }

        DWORD imageCount = WIMGetImageCount(hWim);
        int normalizedIndex = (imageIndex >= 1 && static_cast<DWORD>(imageIndex) <= imageCount) ? imageIndex : 1;

        HANDLE hImage = WIMLoadImage(hWim, normalizedIndex);
        if (!hImage || hImage == INVALID_HANDLE_VALUE) {
            WIMCloseHandle(hWim);
            SetLastErrorMessage(L"Failed to load SSB archive image for content listing");
            return false;
        }

        // Apply image to temp dir so we can enumerate what was captured
        std::wstring applyDir = CreateUniqueTempDir();
        if (applyDir.empty()) {
            WIMCloseHandle(hImage);
            WIMCloseHandle(hWim);
            SetLastErrorMessage(L"Failed to create temp directory for content enumeration");
            return false;
        }

        // Apply - this writes actual files; we enumerate then clean up
        WIMApplyImage(hImage, applyDir.c_str(), 0);

        WIMCloseHandle(hImage);
        WIMCloseHandle(hWim);

        // Enumerate everything that was written using Win32 (no filesystem exceptions)
        EnumerateFilesWin32(applyDir, L"", outFiles);

        // Clean up the temp content
        RemoveDirTree(applyDir);

        return true;
    }
}

extern "C" {

    BACKUPENGINE_API int GetBackupInfo(
        const wchar_t* backupPath,
        wchar_t* buffer,
        int bufferSize) {

        if (!backupPath || !buffer || bufferSize <= 0) {
            SetLastErrorMessage(L"Invalid parameters");
            return -1;
        }

        try {
            if (!fs::exists(backupPath)) {
                SetLastErrorMessage(L"Backup path does not exist");
                return -2;
            }

            std::wostringstream info;

            // Read backup_info.txt if it exists
            std::wstring infoFile = std::wstring(backupPath) + L"\\backup_info.txt";
            if (fs::exists(infoFile)) {
                std::wifstream file(infoFile);
                if (file.is_open()) {
                    std::wstring line;
                    while (std::getline(file, line)) {
                        info << line << L"\n";
                    }
                    file.close();
                }
            }
            else {
                // Generate basic info
                info << L"Backup Information\n";
                info << L"==================\n\n";
                info << L"Location: " << backupPath << L"\n";

                // Count files and calculate size
                size_t fileCount = 0;
                uintmax_t totalSize = 0;

                for (const auto& entry : fs::recursive_directory_iterator(backupPath)) {
                    if (entry.is_regular_file()) {
                        // Skip metadata files
                        if (entry.path().filename() != L"backup_metadata.dat" &&
                            entry.path().filename() != L"backup_info.txt") {
                            fileCount++;
                            try {
                                totalSize += entry.file_size();
                            }
                            catch (...) {}
                        }
                    }
                }

                info << L"Files: " << fileCount << L"\n";
                info << L"Size: " << (totalSize / (1024 * 1024)) << L" MB\n";

                // Get backup date from directory creation time
                auto ftime = fs::last_write_time(backupPath);
                info << L"Type: " << (fs::exists(std::wstring(backupPath) + L"\\backup_metadata.dat") 
                    ? L"File Backup" : L"Unknown") << L"\n";
            }

            std::wstring result = info.str();
            if (result.length() >= (size_t)bufferSize) {
                SetLastErrorMessage(L"Buffer too small");
                return -3;
            }

            wcscpy_s(buffer, bufferSize, result.c_str());
            return 0;
        }
        catch (const fs::filesystem_error&) {
            SetLastErrorMessage(L"Filesystem error reading backup info");
            return -4;
        }
        catch (...) {
            SetLastErrorMessage(L"Exception in GetBackupInfo");
            return -99;
        }
    }

    BACKUPENGINE_API int ListBackupContents(
        const wchar_t* backupPath,
        int imageIndex,
        wchar_t* buffer,
        int bufferSize) {

        if (!backupPath || !buffer || bufferSize <= 0) {
            SetLastErrorMessage(L"Invalid parameters");
            return -1;
        }

        try {
            std::wstring path(backupPath);
            std::vector<std::wstring> files;

            if (IsArchiveFilePath(path)) {
                // Enumerate from archive by applying to temp dir and reading what was written
                if (!TryListArchiveContents(path, imageIndex, files)) {
                    return -2;
                }
            } else {
                // Legacy: enumerate directory directly
                std::error_code ec;
                if (!fs::exists(path, ec) || !fs::is_directory(path, ec)) {
                    SetLastErrorMessage(L"Backup path does not exist");
                    return -2;
                }

                for (const auto& entry : fs::recursive_directory_iterator(path)) {
                    if (entry.is_regular_file()) {
                        std::wstring filename = entry.path().filename().wstring();
                        if (filename == L"backup_metadata.dat" || filename == L"backup_info.txt") {
                            continue;
                        }
                        files.push_back(fs::relative(entry.path(), path).wstring());
                    }
                }
            }

            if (files.empty()) {
                wcscpy_s(buffer, bufferSize, L"(No files in backup)\n");
                return 0;
            }

            std::sort(files.begin(), files.end(), [](const std::wstring& a, const std::wstring& b) {
                return _wcsicmp(a.c_str(), b.c_str()) < 0;
            });

            std::wostringstream contents;
            for (const auto& file : files) {
                contents << file << L"\n";
            }

            std::wstring result = contents.str();

            if (result.length() >= static_cast<size_t>(bufferSize)) {
                SetLastErrorMessage(L"Buffer too small - too many files in backup");
                result = result.substr(0, bufferSize - 50);
                result += L"\n... (list truncated, buffer too small)\n";
                wcscpy_s(buffer, bufferSize, result.c_str());
                return -3;
            }

            wcscpy_s(buffer, bufferSize, result.c_str());
            return 0;
        }
        catch (const fs::filesystem_error&) {
            SetLastErrorMessage(L"Filesystem error listing backup contents");
            return -4;
        }
        catch (...) {
            SetLastErrorMessage(L"Exception in ListBackupContents");
            return -99;
        }
    }
}
