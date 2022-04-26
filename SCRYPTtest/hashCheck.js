const { FirebaseScrypt } = require('firebase-scrypt');
const dotenv = require('dotenv');
dotenv.config();

const hashCheck = async (pass, hash, salt) =>{
//const pass = "Test2Pass234";
//const hash = "JQRFu6aeWV/Z+bLP5RD48qLj/Ia+tqJJ/UGvcmu/ZeiQ6fT/6iPex7zWL+ImgYCi4EdFcITzaHqHnXZ5vGeeAA==";
//const salt = "mcnPvf9P9L8vYQ==";
const firebaseParameters = {
  memCost: Number(process.env.MEM_COST), // replace by your
  rounds: Number(process.env.ROUNDS), // replace by your
  saltSeparator: process.env.SALT_SEPARATOR, // replace by your 
  signerKey: process.env.SIGNER_KEY, // replace by your
}

const scrypt = new FirebaseScrypt(firebaseParameters)

return await scrypt.verify(pass, salt, hash);
}
hashCheck("Test2Pass234","JQRFu6aeWV/Z+bLP5RD48qLj/Ia+tqJJ/UGvcmu/ZeiQ6fT/6iPex7zWL+ImgYCi4EdFcITzaHqHnXZ5vGeeAA==", "mcnPvf9P9L8vYQ==");

module.exports=hashCheck;