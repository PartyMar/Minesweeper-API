const { GenerateMinefield } = require('../utils/generateMinefield');

let gameId = "";
let width = 0;
let height = 0;
let mines_count = 0;
let completed = false;

let field = []
let player_field = []


function GetGameId() {
    return gameId
}
function GetCompleted() {
    return completed
}
function GetPlayerField() {
    return player_field
}
function GetField() {
    return field
}



function GetVariables() {
    return {
        "gameId": gameId,
        "width": width,
        "height": height,
        "mines_count": mines_count,
        "field": player_field,
        "completed": completed,
    }
};

async function NewGameVariables(_gameId, _width, _height, _mines_count) {
    gameId = _gameId;
    width = _width;
    height = _height;
    mines_count = _mines_count;

    try {
        field = await GenerateMinefield(height, width, mines_count);
        player_field = Array.from({ length: height }, () => Array(width).fill(" "));

    } catch (error) {
        throw new Error("Не получилось сгенерировать минное поле", error);
    }
};

async function CheckField(col, row) {
    try {
       
        if (row < 0 || col < 0 || row >= field.length || col >= field[0].length) {
            return 'Вне границ';
        }
        if (field[row][col] === 'M') {
            completed = true;
            field = field.map(row => row.map(cell => cell === 'M' ? 'X' : cell));
            player_field = field;
            return 'Игра завершена';
        } else {
            let visited = new Set();
            RevealEmptySpaces(row, col, visited);
            let win = CheckWinConditions();
            if(win) player_field = field
            return 'Показаны пустые клетки';
        }
    } catch (error) {
        throw new Error("Не получилось проверить поле", error);
    }
}

function RevealEmptySpaces(row, col, visited) {
    if (row < 0 || col < 0 || row >= field.length || col >= field[0].length || visited.has(`${row}-${col}`)) {
        return;
    } else if (field[row][col] !== '0') {
        player_field[row][col] = field[row][col];
        return 'Безопасная клетка открыта';
    }

    visited.add(`${row}-${col}`);
    player_field[row][col] = field[row][col];

    RevealEmptySpaces(row - 1, col, visited);
    RevealEmptySpaces(row + 1, col, visited);
    RevealEmptySpaces(row, col - 1, visited);
    RevealEmptySpaces(row, col + 1, visited);
}

function CheckWinConditions(){

        for (let i = 0; i < player_field.length; i++) {
            for (let j = 0; j < player_field[i].length; j++) {
                if (player_field[i][j] === " " && field[i][j] !== "M") {
                    return false; 
                }
            }
        }
        return true; 
    
}

module.exports = {
    GetVariables,
    NewGameVariables,
    CheckField,
    GetGameId,
    GetCompleted,
    GetPlayerField,
    GetField
};