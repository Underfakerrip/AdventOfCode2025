import path

def run():
    rangeList = []
    ingridientIds = []
    counter = 0
    with open(path.path) as text:
        for textLine in text:
            textLineStripped = textLine.strip()
            if textLineStripped == '': # Empty line
                continue
            if '-' in textLineStripped: # Ingredient id range
                splittedLine = textLineStripped.split('-')
                rangeList.append((int(splittedLine[0]), int(splittedLine[1])))
            else: # Ingredient id
                ingridientIds.append(int(textLineStripped))
    # Check if ingredient id is in range and increment counter if yes
    for ingridentId in ingridientIds:
        for range in rangeList:
            if ingridentId >= range[0] and ingridentId <= range[1]:
                counter += 1
                break
    print(counter)

# Get all id ranges in ascending order (start id) and strip upcoming ranges if they start or end lie in an already stored range
def run2():
    wholeRangeList = getFreshIdRanges()
    rangeList = []
    sum = 0
    for rangeEntry in wholeRangeList:
        rangeStart = rangeEntry[0]
        rangeEnd = rangeEntry[1]
        continueLineLoop = False
        for range in rangeList:
            if rangeStart >= range[0] and rangeStart <= range[1]:
                rangeStart = range[1] + 1
                if rangeStart > rangeEnd:
                    continueLineLoop = True
                    break
            if rangeEnd >= range[0] and rangeEnd <= range[1]:
                rangeEnd = range[0] - 1
                if rangeStart > rangeEnd:
                    continueLineLoop = True
                    break
        if continueLineLoop:
            continue
        sum += rangeEnd - rangeStart + 1
        rangeList.append((rangeStart, rangeEnd))
    print(sum)

def getFreshIdRanges():
    rangeList = []
    with open(path.path) as text:
        for textLine in text:
            textLineStripped = textLine.strip()
            if textLineStripped == '': # Stop at empty line
                break
            splittedLine = textLineStripped.split('-')
            rangeStart = int(splittedLine[0])
            rangeEnd = int(splittedLine[1])
            rangeList.append((rangeStart, rangeEnd))
    rangeList.sort(key=sortHelper)
    return rangeList

def sortHelper(e):
  return e[0]
