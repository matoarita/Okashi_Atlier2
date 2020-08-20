using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
using System.Security.Cryptography;

//データセーブ用。データをAESを用いて暗号化、復号化します。
//暗号化キーと初期化ベクトルはEncryptionKeyとEncryptionIVに対応しています。
//ここは各自変更してください。
//絶対にこのソースコードをそのままGitHub等に上げないでください。暗号がばれます。
//ソースコードを公開したい場合はEncryptionKeyとEncryptionIVをハードコーディングではなく、ファイル等から読み込むようにしてください。
//公開しない場合は問題ありません。

public class Cryptor
{
    static readonly int KeySize = 256;
    static readonly int BlockSize = 128;
    static readonly string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
    static readonly string EncryptionIV = "0123456789ABCDEF";

    public static byte[] Encrypt(byte[] rawData)
    {
        return Encrypt(rawData, EncryptionKey, EncryptionIV);
    }

    public static byte[] Encrypt(byte[] rawData, string key, string iv)
    {
        byte[] result = null;

        using (AesManaged aes = new AesManaged())
        {
            SetAesParams(aes, key, iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream encryptedStream = new MemoryStream())
            {
                using (CryptoStream cryptStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptStream.Write(rawData, 0, rawData.Length);
                }
                result = encryptedStream.ToArray();
            }
        }

        return result;
    }

    public static byte[] Decrypt(byte[] encryptedData)
    {
        return Decrypt(encryptedData, EncryptionKey, EncryptionIV);
    }

    public static byte[] Decrypt(byte[] encryptedData, string key, string iv)
    {
        byte[] result = null;

        using (AesManaged aes = new AesManaged())
        {
            SetAesParams(aes, key, iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream encryptedStream = new MemoryStream(encryptedData))
            {
                using (MemoryStream decryptedStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(decryptedStream);
                    }
                    result = decryptedStream.ToArray();
                }
            }
        }

        return result;
    }

    static void SetAesParams(AesManaged aes, string key, string iv)
    {
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        aes.Key = Encoding.UTF8.GetBytes(CreateKeyFromString(key));
        aes.IV = Encoding.UTF8.GetBytes(CreateIVFromString(iv));
    }

    static string CreateKeyFromString(string str)
    {
        return PaddingString(str, KeySize / 8);
    }

    static string CreateIVFromString(string str)
    {
        return PaddingString(str, BlockSize / 8);
    }

    static string PaddingString(string str, int len)
    {
        const char PaddingCharacter = '.';

        if (str.Length < len)
        {
            string key = str;
            for (int i = 0; i < len - str.Length; ++i)
            {
                key += PaddingCharacter;
            }
            return key;
        }
        else if (str.Length > len)
        {
            return str.Substring(0, len);
        }
        else
        {
            return str;
        }
    }
}