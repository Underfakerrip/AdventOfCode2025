import path

def toInt(text):
    return int(text)

def run():
    matrix = []
    operators = []
    result = 0
    with open(path.path) as text:
        for textLine in text:
            textLineStripped = textLine.strip()
            while "  " in textLineStripped:
                textLineStripped = textLineStripped.replace("  ", " ") # Reduce multiple empty spaces to one
            if '+' in textLineStripped:
                operators = textLineStripped.split(' ')
            else:
                matrix.append(list(map(toInt, textLineStripped.split(' ')))) # Split by the one remaining space per number

    # For each 'column': Create sum or product of included numbers
    for j in range(0, len(matrix[0])):
        taskResult = 0 if operators[j] == '+' else 1
        for i in range(0, len(matrix)):
            taskResult = taskResult + matrix[i][j] if operators[j] == '+' else taskResult * matrix[i][j]
        result += taskResult

    print(result)


def run2():
    matrix = []
    operators = []
    result = 0

    # Create a package (per number) with a Tuple for each digit (containing digit and its index)
    with open(path.path) as text:
        for textLine in text:
            textLine = textLine.rstrip()
            numbersPerLine = [] # Representing all numbers within a line
            if '+' in textLine:
                while "  " in textLine:
                    textLine = textLine.replace("  ", " ")
                operators = textLine.split(' ')
                break
            numberStart = False
            numberTuples = [] # Representing one number with its digit and digitPositions
            # Check for empty spaces which defines end of last and start of new number
            # Until new empty spcae: Put found digits in a package together with its index in line
            for i, char  in enumerate(textLine):
                if char == ' ':
                    if numberStart:
                        numbersPerLine.append(numberTuples)
                    numberStart = False
                else:
                    if numberStart:
                        numberTuples.append((i, int(char)))
                    else:
                        numberTuples = [(i, int(char))]
                        numberStart = True
                if i == len(textLine) - 1:
                    numbersPerLine.append(numberTuples)
            matrix.append(numbersPerLine)

    # Get the new numbers based on their up to down index positions and calculate into the final result
    for j in range(0, len(matrix[0])): # For each column: Append all included number tuples into a list
        currentNumberPackages = []
        for i in range(0, len(matrix)):
            for numberTuple in matrix[i][j]:
                currentNumberPackages.append(numberTuple)

        # Get all indices in the packages
        indices = list(set(map(numberIndexMapper, currentNumberPackages)))
        # From top to buttom:
        strNumber = ""
        lineRes = 0 if operators[j] == '+' else 1
        for index in indices:
            # Check if index is included and concatenate to a string
            for numberPackage in currentNumberPackages:
                if numberPackage[0] == index:
                    strNumber += str(numberPackage[1])
            # Multiply or sum all strings as int into result
            lineRes = lineRes + int(strNumber) if operators[j] == '+' else lineRes * int(strNumber)
            strNumber = ""

        result += lineRes

    print(result)


def numberIndexMapper(e):
  return e[0]
