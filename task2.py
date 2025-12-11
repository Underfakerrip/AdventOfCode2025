import path

def run():
    sumIds = 0
    with open(path.path) as text:
        indexRanges = text.read().split(",")
        for indexRange in indexRanges:
            firstIndex = indexRange.split("-")[0]
            secondIndex = indexRange.split("-")[1]
            if len(firstIndex) == len(secondIndex) and len(firstIndex) % 2 == 1:
                continue
            for x in range(int(firstIndex), int(secondIndex) + 1):
                xStr = str(x)
                if len(xStr) % 2 == 0 and xStr[0:int(len(xStr)/2)] == xStr[int(len(xStr)/2):]:
                    sumIds += x
    print(sumIds)

def run2():
    listIds = list()
    sumIds = 0
    with open(path.path) as text:
        indexRanges = text.read().split(",")
        for indexRange in indexRanges:
            firstIndex = indexRange.split("-")[0]
            secondIndex = indexRange.split("-")[1]
            for x in range(int(firstIndex), int(secondIndex) + 1): # All indexes in range
                if x < 10: # Only two or more digits are capapble of repetitions
                    continue
                xStr = str(x)
                lengthStr = len(xStr)
                upperLimit = (int(lengthStr/2) +1) if int(lengthStr/2) > 2 and not(lengthStr == 3 or lengthStr == 5 or lengthStr == 7 or lengthStr == 11 or lengthStr == 13) else 2
                for slicer in range(1, upperLimit + 1): # All potencial dividers of the string
                    if lengthStr % slicer == 0 and slicer < lengthStr:
                        strParts = getSplitByCharCount(xStr, slicer)
                        oldPart = strParts[0]
                        invalidFound = False
                        for part in strParts[1:]:
                            if (oldPart != part):
                                invalidFound = True
                                continue
                        if not(invalidFound):
                            listIds.append(x)
                            sumIds += x
                            break
    print(sumIds)

def getSplitByCharCount(xStr, charCount):
    strList = list()
    count = 0
    while count <= len(xStr) - charCount:
        strList.append(xStr[count:count+charCount])
        count += charCount
    return strList
