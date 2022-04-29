const express = require("express");
const checkHash = require("./hashCheck");
const serverless = require('serverless-http');
const mongoose = require('mongoose');
const User = require('./models/user');
const app = express();
const router = express.Router();

const dbURI = "mongodb+srv://superUser123:4jEimm5LRYRFMzPw@cluster0.yjdhl.mongodb.net/My-db?retryWrites=true&w=majority";

mongoose.connect(dbURI, { useNewUrlParser: true, useUnifiedTopology: true })
  .then(result => {
    //app.listen(3000)
  })
  .catch(err => console.log(err));

app.use(express.static("public"));
app.use(express.urlencoded({ extended: true }));
app.use(express.json());

app.set("view engine", "ejs");

router.post("/checkHash", (req, res) => {
  let isValid = false;
  if(req.body.password && req.body.email) isValid=true;
  if (isValid) {
    User.findOne({"Email":{$eq:req.body.email}}).then(result => {
      if(result==null) res.send("Error");
      else checkHash(req.body.password, result.HashPass, result.HashSalt).then(isValid=>{
        isValid? res.send(result.ExternalId): res.send("invalid");
     });
    })
    .catch(err => {
      console.log(err);
    });
  } else {
    res.send("Error");
  }
});

router.post("/checkPhone", (req, res) => {
  let isValid = false;
  if(req.body.phone) isValid=true;
  if (isValid) {
    User.findOne({"Phone":{$eq:req.body.phone}}).then(result => {
      if(result==null) res.send("Error");
      else res.send(result.ExternalId);
    })
    .catch(err => {
      console.log(err);
    });
  } else {
    res.send("Error");
  }
});

router.get('/add-user', (req, res) => {
  const user = new User({
    ExternalId: "extId",
    Email: "test2@test.com",
    HashPass: "JQRFu6aeWV/Z+bLP5RD48qLj/Ia+tqJJ/UGvcmu/ZeiQ6fT/6iPex7zWL+ImgYCi4EdFcITzaHqHnXZ5vGeeAA==",
    HashSalt: "mcnPvf9P9L8vYQ==",
    IsMigrate: false,
  })

  user.save()
    .then(result => {
      res.send(result);
    })
    .catch(err => {
      console.log(err);
    });
});

app.use("/", router);

module.exports.handler = serverless(app);