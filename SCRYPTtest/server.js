const express = require("express");
const bodyParser = require('body-parser');
const checkHash = require("./hashCheck");
const app = express();
const router = express.Router();

app.use(express.static("public"));
app.use(express.urlencoded({ extended: true }));
app.use(express.json());


app.use( bodyParser.json() ); 
app.set("view engine", "ejs");

router.get("/checkHash", (req, res) => {
    res.send("Hi");
  });

router.post("/checkHash", (req, res) => {
  let isValid = false;
  if(req.body.pass && req.body.hash && req.body.salt) isValid=true;
  if (isValid) {
    checkHash(req.body.pass, req.body.hash, req.body.salt).then(isValid=>{
       isValid? res.send("valid"): res.send("invalid");
    });
  } else {
    res.send("Error");
  }
});


app.use("/", router);

app.listen(3000);