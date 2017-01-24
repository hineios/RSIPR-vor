var express = require('express');
var app = express();
var router = express.Router();
var path = require('path');

var active = false;

/* GET home page. */ 
router.get('/', function(req, res) {
  //res.render('index', { title: 'Express' });
    res.sendfile(path.join(__dirname+'/../start.html'));
});

/* GET home page. */ 
router.get('/canProceed', function(req, res) {
    res.send(active);
});

router.post('/changeStat', function(req, res) {
    active = req.body.active;
    if (active != "")
        res.send("Change the stat game to: " + active);
    else
        res.send("ERROR")
})


module.exports = router;

