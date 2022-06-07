using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmployeeLibrary.Utilities
{
    public class Utilities
    {

        public static  SEALContext context;
        public static  Encryptor encryptor;
        public static  Decryptor decryptor;
        public static  Evaluator evaluator;
        public static CKKSEncoder encoder;
        public static double scale;
        public static List<double> output;
        static Utilities()
        {
            EncryptionParameters parms = new EncryptionParameters(SchemeType.CKKS);
            ulong polyModulusDegree = 8192;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, new int[] { 60, 40, 40, 60 });
            context = new SEALContext(parms);
            KeyGenerator keygen = new KeyGenerator(context);
            SecretKey secretKey = keygen.SecretKey;
            keygen.CreatePublicKey(out PublicKey publicKey);
            keygen.CreateRelinKeys(out RelinKeys relinKeys);
            encryptor = new Encryptor(context, publicKey);
            evaluator = new Evaluator(context);
            decryptor = new Decryptor(context, secretKey);
            encoder = new CKKSEncoder(context);
            scale = Math.Pow(2.0, 40);
            output = new List<double>();
        }
        public static Ciphertext DoubleToCiphertext(double d)
        {
            Plaintext plaintext = new Plaintext();
            encoder.Encode(d,scale,plaintext);
            Ciphertext ciphertext = new Ciphertext();
            encryptor.Encrypt(plaintext, ciphertext);
            return ciphertext;
        }
        public static double CiphertextToDouble(Ciphertext ciphertext)
        {
            Plaintext plaintext = new Plaintext();
            decryptor.Decrypt(ciphertext,plaintext);
            encoder.Decode(plaintext,output);
            return output[0];

        }
        public static string CiphertextToBase64String(Ciphertext ciphertext)
        {
            using (var ms = new MemoryStream())
            {
                ciphertext.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public static Ciphertext BuildCiphertextFromBase64String(string base64,SEALContext context)
        {
            var payload = Convert.FromBase64String(base64);

            using (var ms = new MemoryStream(payload))
            {
                var ciphertext = new Ciphertext();
                ciphertext.Load(context, ms);

                return ciphertext;
            }
        }
    }
}
