import path

# Map helpter to set True for each @ symbol in matrix
def toBool(character):
    return character == '@'

def run():
    counter = 0
    matrixRows = []
    with open(path.path) as text:
        for textLine in text:
            matrixRows.append(list(map(toBool, textLine.strip())))
    width = len(matrixRows[0])
    height = len(matrixRows)
    for x in range(0, len(matrixRows)):
        for y in range(0, len(matrixRows[0])):
            if (matrixRows[x][y] and checkSourroundingPositions(matrixRows, x, y, height, width)):
                counter += 1
    print(str(counter))


# Repeat first task until no further rolls are found. Switch found rows to False after each iteration
def run2():
    matrixRows = []
    with open(path.path) as text:
        for textLine in text:
            matrixRows.append(list(map(toBool, textLine.strip())))
    width = len(matrixRows[0])
    height = len(matrixRows)

    counterSum = 0 # Sum of all token rolls

    counter = -1 # Sum of token rolls in one iteration
    while counter != 0:
        counter = 0
        indices = []
        for x in range(0, len(matrixRows)):
            for y in range(0, len(matrixRows[0])):
                if (matrixRows[x][y] and checkSourroundingPositions(matrixRows, x, y, height, width)):
                    indices.append((x, y))
                    counter += 1
        counterSum += counter
        for pair in indices:
            matrixRows[pair[0]][pair[1]] = False
    print(str(counterSum))


# Check wether the surrounding ,atrix positions have an '@' (True) and if its in total less than 4 or not
def checkSourroundingPositions(matrix, x, y, height, width): # 1, 0
    count = 0
    if x-1 >= 0:
        if y-1 >= 0 and matrix[x-1][y-1]:
            count += 1
        if matrix[x-1][y]:
            count += 1
        if y+1 < width and matrix[x-1][y+1]:
            count += 1
    if y-1 >= 0 and matrix[x][y-1]:
        count += 1
        if count >= 4:
            return False
    if y+1 < width and matrix[x][y+1]:
        count += 1
        if count >= 4:
            return False
    if x+1 < height:
        if y-1 >= 0 and matrix[x+1][y-1]:
            count += 1
        if count >= 4:
            return False
        if matrix[x+1][y]:
            count += 1
            if count >= 4:
                return False
        if y+1 < width and matrix[x+1][y+1]:
            count += 1
            if count >= 4:
                return False
    return True
