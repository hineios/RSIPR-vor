var express = require('express');
var app = express();
var router = express.Router();
var path = require('path');

var id ="";
var words = [];

router.post('/', function(req, res,next) {
    id = req.body.id;
    words.push(id);
    console.log("Complete POSITION " + id)
    res.send("You set the word in the position: "+ id);
});

router.get('/complete', function(req, res) {
    res.send(words);
    words.pop();
})

/*
var completeBoardWord = function(id){
    console.log(id)
    var word,
        pos;
    for(var i=0; i!=WORDS.length; i++){
        if(WORDS[i]["id"]==id){
            word = WORDS[i]["word"];
            console.log("WORD "+ word)
            pos = WORDS[i]["pos"];
        }
    }
    
    
    for(var i =0; i!=pos.length; i++) {
        $("#"+pos[i]).val(word[i]);
    }
}

setInterval(function(){
    if(id!=""){
        completeBoardWord(id);
        id ="";
    }
},1000)*/

module.exports = router;


