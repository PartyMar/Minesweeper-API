const { v4: uuidv4 } = require('uuid');


function GenerateGameId() {
  const uuid = uuidv4();
  const gameId = `${uuid.substr(0, 8)}-${uuid.substr(9, 4)}-${uuid.substr(14, 4)}-${uuid.substr(19, 4)}-${uuid.substr(24)}`;

  return gameId.toUpperCase();
};

module.exports = { GenerateGameId };