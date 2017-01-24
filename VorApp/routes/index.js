var express = require('express');
var app = express();
var router = express.Router();
var path = require('path');

var login = false;

/* GET home page. */
router.get('/', function(req, res) {
  //res.render('index', { title: 'Express' });
    console.log("REQUEST")
    res.sendfile(path.join(__dirname+'/../index.html'));
});

module.exports = router;

