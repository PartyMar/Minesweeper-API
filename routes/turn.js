const { GetVariables, CheckField } = require("../utils/variables");


async function Turn(req, res) {
    try {
        const { col, row } = req.body;


        await CheckField(col, row);

        var obj = GetVariables();
        res.status(200).json(obj);

    } catch (error) {
        console.error("Произошла непредвиденная ошибка в Turn: ", error);
        res.status(400).json({ error: 'Произошла непредвиденная ошибка' });
    }
};

module.exports = { Turn };
