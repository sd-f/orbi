package foundation.softwaredesign.orbi.service.auth;

import org.apache.commons.codec.binary.Base64;

import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.spec.InvalidKeySpecException;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 *     from http://stackoverflow.com/questions/2860943/how-can-i-hash-a-password-in-java
 */
public class Hasher {

        // The higher the number of iterations the more
        // expensive computing the hash is for us and
        // also for an attacker.
        private static final int iterations = 20*1000;
        private static final int saltLen = 32;
        private static final int desiredKeyLen = 256;

        /** Computes a salted PBKDF2 hash of given plaintext password
         suitable for storing in a database.
         Empty passwords are not supported. */
        public static String getSaltedHash(char[] password) throws NoSuchAlgorithmException, InvalidKeySpecException {
            byte[] salt = SecureRandom.getInstance("SHA1PRNG").generateSeed(saltLen);
            // store the salt with the password
            return Base64.encodeBase64String(salt) + "$" + hash(password, salt);
        }

        /** Checks whether given plaintext password corresponds
         to a stored salted hash of the password. */
        public static boolean check(char[] password, String stored) throws InvalidKeySpecException, NoSuchAlgorithmException {
            String[] saltAndPass = stored.split("\\$");
            if (saltAndPass.length != 2) {
                throw new IllegalStateException(
                        "The stored password have the form 'salt$hash'");
            }
            String hashOfInput = hash(password, Base64.decodeBase64(saltAndPass[0]));
            return hashOfInput.equals(saltAndPass[1]);

        }

        // using PBKDF2 from Sun, an alternative is https://github.com/wg/scrypt
        // cf. http://www.unlimitednovelty.com/2012/03/dont-use-bcrypt.html
        private static String hash(char[] password, byte[] salt) throws NoSuchAlgorithmException, InvalidKeySpecException {
            if (password == null || password.length == 0)
                throw new IllegalArgumentException("Empty passwords are not supported.");
            SecretKeyFactory f = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
            SecretKey key = f.generateSecret(new PBEKeySpec(
                    password, salt, iterations, desiredKeyLen)
            );
            return Base64.encodeBase64String(key.getEncoded());
        }

        public static String hashMD5(String token) throws NoSuchAlgorithmException {
            MessageDigest md = MessageDigest.getInstance("MD5");
            //Add password bytes to digest
            md.update(token.getBytes());
            //Get the hash's bytes
            byte[] bytes = md.digest();
            //This bytes[] has bytes in decimal format;
            //Convert it to hexadecimal format
            StringBuilder sb = new StringBuilder();
            for(int i=0; i< bytes.length ;i++)
            {
                sb.append(Integer.toString((bytes[i] & 0xff) + 0x100, 16).substring(1));
            }
            //Get complete hashed password in hex format
             return sb.toString();
        }
}
