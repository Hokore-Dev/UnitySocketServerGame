var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

server.listen(3000);

app.get('/', function (req, res) {
    res.send('hello world!');
});

var clients = [];

io.on('connect', function(socket)
{
    var currentPlayer = {};
    currentPlayer.name = 'unknown';

    /**
     * 유저 접속 이벤트, 기존 유저 정보를 보냄
     */
    socket.on('user_connect', function (data) {
        currentPlayer.name     = data.name;
        currentPlayer.position = data.position;

        console.log(currentPlayer.name + ' recv player connect');
        clients.push(currentPlayer); 
        for (var i = 0; i < clients.length; i++)
        {
            var playerConnected = {
                name:clients[i].name,
                position:clients[i].position,
            };
            socket.emit('other_user_connect', playerConnected);
            console.log(currentPlayer.name + ' emit: other player connected: ' + JSON.stringify(playerConnected));
        }
        socket.broadcast.emit('other_user_connect', currentPlayer);
    });

    /**
     * 유저 정보를 다른 유저에계 알림
     */
    socket.on('player_update', function (data) {
        console.log('recv: move: ' + JSON.stringify(data));
        currentPlayer.position = data.position;
        socket.broadcast.emit('other_player_update', currentPlayer);
    });

    /**
     * 유저의 로그아웃을 다른 유저들에게 전송 
     */
    socket.on('disconnect', function () {
        console.log(currentPlayer.name + ' recv: disconnect ' + currentPlayer.name);
        socket.broadcast.emit('other_user_disconnect', currentPlayer);
        console.log(currentPlayer.name + ' bcst: other player disconnected' + JSON.stringify(currentPlayer));
        for (var i = 0; i < clients.length; i++)
        {
            if (clients[i].name === currentPlayer.name)
            {
                clients.splice(i, 1);
            }
        }
    });
});