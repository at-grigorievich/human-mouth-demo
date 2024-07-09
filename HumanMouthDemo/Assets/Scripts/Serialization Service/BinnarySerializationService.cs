#nullable enable

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ATG.Serialization
{
    public static class BinnarySerializationService
    {
        private static BinaryFormatter converter = new BinaryFormatter();

        public static T? Read<T>(string path) where T : class
        {
            T? result = null;

            if (File.Exists(path))
            {
                using (FileStream stream = File.Open(path, FileMode.Open))
                {
                    result = converter.Deserialize(stream) as T;
                }
            }

            return result;
        }

        public static void Write<T>(string path, T dto) where T : class
        {
            using (FileStream stream = File.Create(path))
            {
                converter.Serialize(stream, dto);
            }

#if UNITY_EDITOR
            Debug.Log("finish writing...");
#endif
        }

        public static void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

#if UNITY_EDITOR
            Debug.Log("finish deleting...");
#endif
        }
    }
}