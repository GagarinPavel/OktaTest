// See https://aka.ms/new-console-template for more information
using FirebaseAdmin;
using FirebaseAdmin.Auth.Hash;
using Google.Apis.Auth.OAuth2;
using System.Text;

var scrypt = new Scrypt();
scrypt.SaltSeparator = Encoding.ASCII.GetBytes("Bw==");
scrypt.Key = Encoding.ASCII.GetBytes("KZXk/oLM9GLhX6u48F0XXPKxeUUwlSzStJuT8jLs1/y7b52MTYHuf5FhB2eENtHGc3vgcAzjew7D3PJT1mb1wQ==");
scrypt.MemoryCost = 14;
scrypt.Rounds = 8;
