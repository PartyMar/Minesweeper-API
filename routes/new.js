const { GetVariables, NewGameVariables, GetPlayerField, GetField } = require('../utils/variables');
const { GenerateGameId } = require('../utils/generateId');


async function NewGame(req, res) {
    try {
        const { width, height, mines_count } = req.body;
        const newId = GenerateGameId();
        await NewGameVariables(newId, width, height, mines_count);
        var obj = GetVariables();

        res.status(200).json(obj);

    } catch (error) {
        console.error("Произошла непредвиденная ошибка в NewGame: ", error);
        res.status(400).json({ error: 'Произошла непредвиденная ошибка' });
    }
};

module.exports = { NewGame };