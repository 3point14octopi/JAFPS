using System;


namespace JAFPS_API
{
    class Parser
    {

        private ConversionHelpers WConvert = new ConversionHelpers();

        /// <summary>
        /// assembles WD packages
        /// </summary>
        /// <param name="contents">array of primitive types or strings</param>
        /// <returns> packaged data as an array of bytes </returns>
        public byte[] Convert(object[] contents)
        {
            int objectsParsed = 0;
            byte[] data = new byte[1024];
            
            int skip = 2;
            for(int i = 0; i < contents.Length; i++)
            {
                byte[] elm = new byte[0];

                //parse object into bytes
                short tCode = JAFPS_CONVERSIONCODES.CodeForName(contents[i].GetType().FullName);
             
                if(tCode > 9)
                {
                    elm = WConvert.EncodeStringArray(contents[i] as string[]);
                }else if(tCode > 5)
                {
                    var type = (JAFPS_CONVERSIONCODES.GetTypeFromCode((short)(tCode - 5)));
                    dynamic x = Activator.CreateInstance(type);
                    elm = WConvert.EncodeBasicArray(x, contents[i]);
                }else if(tCode > 4)
                {
                    elm = WConvert.EncodeString(contents[i] as string);
                }else if(tCode > 0)
                {
                    var type = (contents[i].GetType());
                    dynamic x = Activator.CreateInstance(type);
                    elm = WConvert.EncodeBasicType(x, contents[i]);
                }
                else
                {
                    Console.WriteLine("PARSER ERROR: could not locate method to parse {0}.", contents[i].GetType().FullName);
                }
                

                //copy object to data buffer if it fits, 
                if(elm.Length < (data.Length - skip))
                {
                    if(elm.Length > 0)objectsParsed++;
                    Array.Copy(elm, 0, data, skip, elm.Length);
                    skip += elm.Length;
                }
                else //otherwise exit the loop
                {
                    i = contents.Length;
                }
                
            }

            //write how many objects were successfully copied
            Buffer.BlockCopy(BitConverter.GetBytes((short)objectsParsed), 0, data, 0, 2);

            //append data
            byte[] copy = new byte[skip];
            Array.Copy(data, copy, skip);
            data = copy;

            //return parsed data
            return data;
        }

        /// <summary>
        /// decodes WD packages
        /// </summary>
        /// <param name="data"> data received </param>
        /// <returns> array of object commands</returns>
        public object[] Deconvert(byte[] data)
        {

            int originalByteCount = data.Length;
            int objectsDecoded = 0;
            object[] decodedData = new object[BitConverter.ToInt16(data, 0)];
            short bToAdd = 0;

            for(short currentByte = 2; currentByte < originalByteCount - 1;)
            {
                //get code for next object in the buffer
                short obCode = BitConverter.ToInt16(data, currentByte);
                currentByte += 2;

                //remove identifier from remaining data
                byte[] remaining = new byte[data.Length - currentByte];
                Buffer.BlockCopy(data, currentByte, remaining, 0, remaining.Length);
                

                if(obCode > 9)
                {
                    decodedData[objectsDecoded] = WConvert.DecodeStringArray(remaining, out bToAdd);
                }else if(obCode > 5)
                {
                    var type = JAFPS_CONVERSIONCODES.GetTypeFromCode((short)(obCode - 5));
                    dynamic x = Activator.CreateInstance(type);
                    decodedData[objectsDecoded] = WConvert.DecodeBasicArray(x, remaining, out bToAdd);
                }else if (obCode > 4)
                {
                    decodedData[objectsDecoded] = WConvert.DecodeString(remaining, out bToAdd);
                }else if(obCode > 0)
                {
                    var type = JAFPS_CONVERSIONCODES.GetTypeFromCode(obCode);
                    dynamic x = Activator.CreateInstance(type);
                    decodedData[objectsDecoded] = WConvert.DecodeBasicType(x, remaining, out bToAdd);
                }
                else
                {
                    Console.WriteLine("PARSER ERROR: could not locate method to parse object code \"{0}\"", obCode.ToString());
                }

                objectsDecoded++;

                currentByte += bToAdd;
            }
                
            return decodedData;

        }
    }
}