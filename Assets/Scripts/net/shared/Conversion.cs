///this is probably the worst code you have ever seen in your life
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace JAFPS_API
{

    public class ConversionHelpers
    {

        public byte[] EncodeBasicType<T>(T oType, object obj)
        {
            T[] ar = new T[] { (T)obj };
            byte[] bytes = new byte[Marshal.SizeOf<T>() + 2];

            Buffer.BlockCopy((BitConverter.GetBytes(JAFPS_CONVERSIONCODES.CodeForName(ar[0].GetType().FullName))), 0, bytes, 0, 2);
            Buffer.BlockCopy(ar, 0, bytes, 2, bytes.Length - 2);

            return bytes;
        }

        public byte[] EncodeString(string toEncode)
        {
            byte[] bytes = new byte[toEncode.Length + 4];

            Buffer.BlockCopy((BitConverter.GetBytes(JAFPS_CONVERSIONCODES.CodeForName("System.String"))), 0, bytes, 0, 2);
            Buffer.BlockCopy((BitConverter.GetBytes((short)toEncode.Length)), 0, bytes, 2, 2);
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(toEncode), 0, bytes, 4, toEncode.Length);

            return bytes;
        }

        public byte[] EncodeBasicArray<T>(T arType, object obAr)
        {
            T[] array = obAr as T[];
            byte[] bytes = new byte[(Marshal.SizeOf<T>() * array.Length) +4];
            
            Buffer.BlockCopy((BitConverter.GetBytes(JAFPS_CONVERSIONCODES.CodeForName(array.GetType().FullName))), 0, bytes, 0, 2);
            
            Buffer.BlockCopy(BitConverter.GetBytes((short)array.Length), 0, bytes, 2, 2);
            Buffer.BlockCopy(array, 0, bytes, 4, bytes.Length - 4);

            return bytes;
        }

        public byte[] EncodeStringArray(string[] toEncode)
        {
            byte[] fullBuffer = new byte[1024];//impossible to fill 
            Buffer.BlockCopy(BitConverter.GetBytes((short)JAFPS_CONVERSIONCODES.CodeForName(toEncode.GetType().FullName)), 0, fullBuffer, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((short)toEncode.Length), 0, fullBuffer, 2, 2);
            int byteCount = 4;

            for(int i = 0; i < toEncode.Length; i++)
            {
                byte[] curString = Encoding.ASCII.GetBytes(toEncode[i]);
                Buffer.BlockCopy(BitConverter.GetBytes((short)curString.Length), 0, fullBuffer, byteCount, 2);
                byteCount += 2;
                curString.CopyTo(fullBuffer, byteCount);
                byteCount += curString.Length;
            }

            byte[] returnArray = new byte[byteCount];
            Array.Copy(fullBuffer, returnArray, byteCount);

            return returnArray;
        }

        public object DecodeBasicType<T>(T oType, byte[] bytes, out short totalSize)
        {
            T[] copyAr = new T[1];
            totalSize = (short)Marshal.SizeOf<T>();

            Buffer.BlockCopy(bytes, 0, copyAr, 0, totalSize);

            return copyAr[0];
        }

        public object DecodeString(byte[] bytes, out short totalSize)
        {
            totalSize = BitConverter.ToInt16(bytes, 0);
            totalSize += 2;

            return Encoding.ASCII.GetString(bytes, 2, totalSize - 2);
        }

        public object DecodeBasicArray<T>(T elementType, byte[] source, out short totalSize)
        {
            T[] array = new T[BitConverter.ToInt16(source, 0)];
            totalSize = (short)(Marshal.SizeOf<T>() * array.Length);

            Buffer.BlockCopy(source, 2, array, 0, totalSize);

            totalSize += 2;
            return array;
        }

        public object DecodeStringArray(byte[] bytes, out short totalSize)
        {
            short arLength = BitConverter.ToInt16(bytes, 0); 
            string[] decodedArray = new string[arLength];

            totalSize = 2;

            for(int i = 0; i < decodedArray.Length; i++)
            {
                short elLength = BitConverter.ToInt16(bytes, totalSize);
                totalSize += 2;
                decodedArray[i] = Encoding.ASCII.GetString(bytes, totalSize, elLength);
                totalSize += elLength;
            }

            return decodedArray;
        }
    }
}