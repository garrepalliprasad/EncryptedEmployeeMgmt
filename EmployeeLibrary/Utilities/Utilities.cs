using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmployeeLibrary.Utilities
{
    public class Utilities
    {
        private readonly SEALContext _context;
        private readonly Encryptor _encryptor;
        private readonly Decryptor _decryptor;
        private readonly Evaluator _evaluator;
        private readonly CKKSEncoder _encoder;
        private readonly double _scale;
        private readonly List<double> _output;
        public Utilities()
        {
            EncryptionParameters parms = new EncryptionParameters(SchemeType.CKKS);
            ulong polyModulusDegree = 8192;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, new int[] { 60, 40, 40, 60 });
            _context = new SEALContext(parms);
            KeyGenerator keygen = new KeyGenerator(_context);
            SecretKey secretKey = keygen.SecretKey;
            keygen.CreatePublicKey(out PublicKey publicKey);
            keygen.CreateRelinKeys(out RelinKeys relinKeys);
            _encryptor = new Encryptor(_context, publicKey);
            _evaluator = new Evaluator(_context);
            _decryptor = new Decryptor(_context, secretKey);
            _encoder = new CKKSEncoder(_context);
            _scale = Math.Pow(2.0, 40);
            _output = new List<double>();
        }
        //public SEALContext Context
        //{
        //    get { return _context; }
        //}
        //public Encryptor Encryptor
        //{
        //    get { return _encryptor; }
        //}
        //public Decryptor Decryptor
        //{
        //    get { return _decryptor; }
        //}
        public Evaluator Evaluator
        {
            get { return _evaluator; }
        }
        //public CKKSEncoder Encoder
        //{
        //    get { return _encoder; }
        //}
        public Ciphertext DoubleToCiphertext(double d)
        {
            Plaintext plaintext = new Plaintext();
            _encoder.Encode(d,_scale,plaintext);
            Ciphertext ciphertext = new Ciphertext();
            _encryptor.Encrypt(plaintext, ciphertext);
            return ciphertext;
        }
        public double CiphertextToDouble(Ciphertext ciphertext)
        {
            Plaintext plaintext = new Plaintext();
            _decryptor.Decrypt(ciphertext,plaintext);
            _encoder.Decode(plaintext,_output);
            return _output[0];

        }
        public string CiphertextToBase64String(Ciphertext ciphertext)
        {
            using (var ms = new MemoryStream())
            {
                ciphertext.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public Ciphertext BuildCiphertextFromBase64String(string base64)
        {
            var payload = Convert.FromBase64String(base64);

            using (var ms = new MemoryStream(payload))
            {
                var ciphertext = new Ciphertext();
                ciphertext.Load(_context, ms);

                return ciphertext;
            }
        }
    }
}
