const mongoose = require('mongoose');
const Schema = mongoose.Schema;

const userSchema = new Schema({
  ExternalId: {
    type: String,
    required: true,
  },
  Email: {
    type: String,
    required: false,
  },
  HashPass: {
    type: String,
    required: false,
  },
  HashSalt: {
    type: String,
    required: false,
  },
  Phone:{
    type: String,
    required: false
  },
  IsMigrated:{
    type: Boolean,
    required: true,
    default: false
  },
}, { timestamps: true });

const User = mongoose.model('User', userSchema);
module.exports = User;