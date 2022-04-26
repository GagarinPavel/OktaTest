const { FirebaseScrypt } = require('firebase-scrypt');

const pass = "Test2Pass234";
const hash = "JQRFu6aeWV/Z+bLP5RD48qLj/Ia+tqJJ/UGvcmu/ZeiQ6fT/6iPex7zWL+ImgYCi4EdFcITzaHqHnXZ5vGeeAA==";
const salt = "mcnPvf9P9L8vYQ==";
const firebaseParameters = {
  memCost: 14, // replace by your
  rounds: 8, // replace by your
  saltSeparator: 'Bw==', // replace by your 
  signerKey: 'KZXk/oLM9GLhX6u48F0XXPKxeUUwlSzStJuT8jLs1/y7b52MTYHuf5FhB2eENtHGc3vgcAzjew7D3PJT1mb1wQ==', // replace by your
}

const scrypt = new FirebaseScrypt(firebaseParameters)

scrypt.verify(pass, salt, hash)
  .then(isValid => isValid ? console.log('Valid !') : console.log('Not valid !'))