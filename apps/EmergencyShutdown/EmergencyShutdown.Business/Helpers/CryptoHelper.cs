//using System.Reflection;
//using System.Security.Cryptography;
//using System.Text;

//namespace EmergencyShutdown.Business.Helpers
//{
//    public class CryptoHelper
//    {


//        public CryptoHelper()
//        {
//            Key_Name = Assembly.GetExecutingAssembly().GetName().Name;
//            Key_Size = 4096;


//        }

//        /// <summary>
//        /// Name of the key container
//        /// </summary>
//        public string Key_Name { private get; set; }

//        /// <summary>
//        /// The size of the key to use in bits
//        /// </summary>
//        public int Key_Size { private get; set; }



//        private RSACryptoServiceProvider rsa;


//        /// <summary>
//        /// Create a new container for the keys
//        /// </summary>
//        /// <returns>true if container was successfully created</returns>
//        public bool CreateContainer()
//        {
//            try
//            {
//                var cp = new CspParameters
//                    {
//                        KeyContainerName = Key_Name,
//                        Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseArchivableKey
//                                                          | CspProviderFlags.UseMachineKeyStore,
//                        //Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseArchivableKey
//                        //                                  ,
//                        KeyNumber = 1
//                    };

//                //CryptoKeyAccessRule rule = new CryptoKeyAccessRule("everyone", CryptoKeyRights.FullControl, AccessControlType.Allow);

//                //cp.CryptoKeySecurity = new CryptoKeySecurity();
//                //cp.CryptoKeySecurity.SetAccessRule(rule);


//                rsa = new RSACryptoServiceProvider(Key_Size, cp){PersistKeyInCsp = true};

//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }


//        private byte[] data;

//        public string Encrypt(string plainText)
//        {
            

//            var plaintext = Encoding.UTF8.GetBytes(plainText);
//            var encryptedtext = rsa.Encrypt(plaintext, false);
//            data = encryptedtext;
//            return Convert.ToBase64String(encryptedtext); ;
//        }

//        public string Decrypt(string cryptedText)
//        {
//            var encryptedtext = Convert.FromBase64String(cryptedText);

//            //for (var i = 0; i < encryptedtext.Length; i++)
//            //{
//            //    if (data[i] != encryptedtext[i])
//            //    {
//            //        return null;
//            //    }

//            //}
            


//            var plaintext = rsa.Decrypt(encryptedtext, false);
//            return Encoding.UTF8.GetString(plaintext);
//        }



//        /// <summary>
//        /// Check if the container for the keys already exists
//        /// </summary>
//        /// <returns>true if container exists</returns>
//        public bool CheckIfContainerExists()
//        {
//            var cspParams = new CspParameters
//            {
//                Flags = CspProviderFlags.UseMachineKeyStore,
//                KeyContainerName = Key_Name,
//                KeyNumber = 1
//            };

//            try
//            {
//                rsa = new RSACryptoServiceProvider(Key_Size, cspParams) { PersistKeyInCsp = true };
//            }
//            catch (Exception ex)
//            {
//                //Debug.Print(ex.Message);
//                return false;
//            }
//            return true;
//        }

//        /// <summary>
//        /// Delete the container
//        /// </summary>
//        public void DeleteContainer()
//        {
//            try
//            {
//                var cp = new CspParameters
//                {
//                    Flags = CspProviderFlags.UseMachineKeyStore,
//                    KeyContainerName = Key_Name,
//                    KeyNumber = 1
//                };
                

//                // Create a new instance of RSACryptoServiceProvider that accesses
//                // the key container.
//                var rsa = new RSACryptoServiceProvider(cp) { PersistKeyInCsp = false };
//                rsa.PersistKeyInCsp = false;
//                // Delete the key entry in the container.

//                // Call Clear to release resources and delete the key from the container.
//                rsa.Clear();
//            }
//            catch 
//            {
//            }
//        }


//        /// <summary>
//        /// Save keys to a XML file
//        /// </summary>
//        /// <param name="fileName"></param>
//        internal void SaveKeys(string fileName)
//        {

//            if (File.Exists(fileName)) File.Delete(fileName);

//            var cp = new CspParameters
//            {
//                KeyContainerName = Key_Name,
//                Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseArchivableKey
//                                                  | CspProviderFlags.UseMachineKeyStore,
//                KeyNumber = 1
//            };
//            using (var rsa = new RSACryptoServiceProvider(Key_Size, cp))
//            {
//                using (
//                    var fs = new FileStream(fileName, FileMode.Create))
//                {
//                    using (StreamWriter sw = new StreamWriter(fs))
//                    {
//                        rsa.PersistKeyInCsp = true;
//                        sw.WriteLine(rsa.ToXmlString(true));
//                    }
//                }
//            }

//        }
//    }
//}
