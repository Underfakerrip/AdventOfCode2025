import path

def run():
    lineIndex = 0
    beamFields = set()
    counter = 0
    with open(path.path) as text:
        for textLine in text:
            for i, char  in enumerate(textLine):
                if char == 'S':
                    beamFields.add((lineIndex, i))
                if lineIndex == 0:
                    continue
                elementBefore = next((x for x in beamFields if x[0] == lineIndex-1 and x[1] == i), None)
                if elementBefore is not None:
                    if char == '^': # Beam before this field
                        counter += 1
                        beamFields.add((lineIndex+1, i-1))
                        beamFields.add((lineIndex+1, i+1))
                    else:
                        beamFields.add((lineIndex, i))
            lineIndex += 1

    print(counter)


# Remember how many paths go through a field and add this amount to the result when encountering a splitter
def run2():
    lineIndex = 0
    beamFields = []
    counter = 1 # There is at least one way
    with open(path.path) as text:
        for textLine in text:
            for i, char  in enumerate(textLine.strip()):
                if char == 'S':
                    beamFields.append([lineIndex, i, 1])
                if lineIndex == 0: # Only 'S' is interessting in line 0
                    continue

                # Find entry that is part of a path
                foundEntry = next((x for x in beamFields if x[0] == lineIndex-1 and x[1] == i), None)
                res = 0 if foundEntry is None else foundEntry[2] # Number of path through this field
                if char == '^':
                    if res > 0: # Paths before this field exist
                        counter += res # Add the amount of paths to our result

                        # Find the next fields below and add them to our list of paths with the number of paths if not already existing,
                        # otherwise (if already existing) increase its amount accordingly
                        beamField = next((x for x in beamFields if x[0] == lineIndex+1 and x[1] == i-1), None) # Left field below
                        if beamField is not None:
                            beamField[2] += res
                        else:
                            beamFields.append([lineIndex+1, i-1, res])
                        beamField = next((x for x in beamFields if x[0] == lineIndex+1 and x[1] == i+1), None) # Right field below
                        if beamField is not None:
                            beamField[2] += res
                        else:
                            beamFields.append([lineIndex+1, i+1, res])
                elif res > 0: # For each non splitted field: Take the number of paths from the above field
                    beamField = next((x for x in beamFields if x[0] == lineIndex and x[1] == i), None)
                    if beamField is not None:
                        beamField[2] += res
                    else:
                        beamFields.append([lineIndex, i, res])
            lineIndex += 1

    print(counter)
