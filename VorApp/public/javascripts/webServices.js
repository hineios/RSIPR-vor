var webservices;
webservices = {
    _escapeString: function(str) {
        return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
    },
    
    request: function(endpoint, params, callback) {
        $.ajax({
            url: WS_ENDPOINT + endpoint,
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            processData: false,
            data: JSON.stringify(params),
            success: function(data){
            	callback(data);
            },
            error: function(error){
                callback(error);
            }
        });
    }
};

VorGame = {
    
    LookAtTable: function(params, callback){
        webservices.request("", params, callback);
    },
    
    SendWordID: function(params, callback){
        webservices.request("", params, callback);
    },
    
    SendPlayersNames: function(params, callback){
        webservices.request("/playerNames", params, callback)
    },
    
    FoundWord: function(params, callback){
        webservices.request("/wordAccepted", params, callback);
    },
    
    WordDeclined: function(params, callback){
        webservices.request("/wordDeclined", params, callback);
    },
    
    SelectedWord: function(params, callback){
        webservices.request("/selectedWord", params, callback);
    },
    
    GameConnection: function(params, callback){
        webservices.request("/gameConnection", params, callback);
    },
    
    TipNotSelected: function(params, callback){
        webservices.request("/tipNotSelected", params, callback);
    }/*,
    
    GameStarted: function(params, callback){
        webservices.request("/gameStarted", params, callback);
    },
    
    GameEnded: function(params, callback){
        webservices.request("/gameEnded", params, callback);
    }*/
}