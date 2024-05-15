

async function GenerateMinefield(height, width, mineCount) {

    let minefield = Array.from({ length: height }, () => Array(width).fill(0+""));

    let minesPlaced = 0;
    while (minesPlaced < mineCount) {
        let row = Math.floor(Math.random() * height);
        let col = Math.floor(Math.random() * width);

        if (minefield[row][col] !== 'M') {
            minefield[row][col] = 'M';
            minesPlaced++;
        }
    }

    for (let i = 0; i < height; i++) {
        for (let j = 0; j < width; j++) {
            if (minefield[i][j] === 'M') continue;

            let count = 0;
            for (let x = -1; x <= 1; x++) {
                for (let y = -1; y <= 1; y++) {
                    if (i + x >= 0 && i + x < height && j + y >= 0 && j + y < width) {
                        if (minefield[i + x][j + y] === 'M') {
                            count++;
                        }
                    }
                }
            }
            minefield[i][j] = count+"";
        }
    }
  
    return minefield;
}

module.exports = { GenerateMinefield };