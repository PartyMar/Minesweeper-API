const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors')

const { NewGame } = require('./routes/new');
const { Turn } = require('./routes/turn');


const app = express();
app.use(bodyParser.json());
app.use(cors())

const port = process.env.PORT || 3000;

app.post('/new', NewGame)

app.post('/turn', Turn)


app.listen(port, () => {
  console.log(`Server is running on port ${port}`);
});
