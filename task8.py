from math import sqrt
import path

def retrieveCoordinates():
    coordinatesList = []
    with open(path.path) as text:
        for textLine in text:
            splittedString = textLine.strip().split(',')
            coordinatesList.append((int(splittedString[0]), int(splittedString[1]), int(splittedString[2])))
    return coordinatesList

# Calculate all distances
def calcSortedDistances(coordinatesList):
    distances = []
    for i, coordinates in enumerate(coordinatesList):
        for j in range(i + 1, len(coordinatesList)):
            distances.append((i, j, calculateDistance(coordinates, coordinatesList[j])))
    # Sort distances
    distances.sort(key = distanceComparator)
    return distances

def run():
    circuits = list(list())
    coordinatesList = retrieveCoordinates()
    distances = calcSortedDistances(coordinatesList)

    # In order add to circuits
    for counter in range(1000):
        coord1Index = distances[counter][0]
        coord2Index = distances[counter][1]

        circuitIndices = findCircuitIndices(circuits, coord1Index, coord2Index)
        firstCircuitIndex = circuitIndices[0]
        secondCircuitIndex = circuitIndices[1]

        attachCoordinatesToCircuits(circuits, firstCircuitIndex, secondCircuitIndex, coord1Index, coord2Index)

    circuitsCoordinates = list(map(toCircuitCoordinatesList, circuits)) # Get only the indices not the connections
    circuitsCoordinates.sort(key = circuitComparator) # Sort by the number of indices per circuit
    product = 1
    for i in range(3):
        product *= len(circuitsCoordinates[i])
    print("Produkt: " + str(product))


def run2():
    circuits = list(list())
    coordinatesList = retrieveCoordinates()
    distances = calcSortedDistances(coordinatesList)

    takenCoordinates = []
    lastCoordinatesIndices = (1, 2) # Dummy value

    # In order add to circuits
    for index, currentDistance in enumerate(distances):
        coord1Index = currentDistance[0]
        coord2Index = currentDistance[1]

        circuitIndices = findCircuitIndices(circuits, coord1Index, coord2Index)
        firstCircuitIndex = circuitIndices[0]
        secondCircuitIndex = circuitIndices[1]

        attachCoordinatesToCircuits(circuits, firstCircuitIndex, secondCircuitIndex, coord1Index, coord2Index)

        takenCoordinates.append(coord1Index)
        takenCoordinates.append(coord2Index)
        if len(set(takenCoordinates)) == len(coordinatesList) and len(circuits) == 1:
            lastCoordinatesIndices = (coord1Index, coord2Index)
            break

    res = coordinatesList[lastCoordinatesIndices[0]][0] * coordinatesList[lastCoordinatesIndices[1]][0]
    print("Ergebnis: " + str(res))


# Check if the coordinates are already contained by circuits and return their indices
def findCircuitIndices(circuits, coord1Index, coord2Index):
    firstCircuitIndex = -1
    secondCircuitIndex = -1
    for i, circuit in enumerate(circuits):
        if next((y for y in circuit if y[0] == coord1Index or y[1] == coord1Index), None) is not None:
            firstCircuitIndex = i
        if next((y for y in circuit if y[0] == coord2Index or y[1] == coord2Index), None) is not None:
            secondCircuitIndex = i
        if firstCircuitIndex != -1 and secondCircuitIndex != -1:
            break
    return (firstCircuitIndex, secondCircuitIndex)

# Apply coordinate sto circuits either in creating new circuit or add/merge into existing, if coordinates are already element of circuit(s)
def attachCoordinatesToCircuits(circuits, firstCircuitIndex, secondCircuitIndex, coord1Index, coord2Index):
    if firstCircuitIndex == -1 and secondCircuitIndex == -1:
        circuits.append([(coord1Index, coord2Index)])
    else:
        if firstCircuitIndex > -1 and secondCircuitIndex > -1: # Both coordinates were found in one circuit each
            if firstCircuitIndex != secondCircuitIndex: # Merge
                newCircuit = circuits[firstCircuitIndex] + circuits[secondCircuitIndex]
                newCircuit.append((coord1Index, coord2Index))
                circuits.append(newCircuit)
                if (firstCircuitIndex > secondCircuitIndex):
                    circuits.remove(circuits[firstCircuitIndex])
                    circuits.remove(circuits[secondCircuitIndex])
                else:
                    circuits.remove(circuits[secondCircuitIndex])
                    circuits.remove(circuits[firstCircuitIndex])
        elif firstCircuitIndex == -1: # Add in second circuit
            circuits[secondCircuitIndex].append((coord1Index, coord2Index))
        else: # Add in first circuit
            circuits[firstCircuitIndex].append((coord1Index, coord2Index))

# Get the coordinates indices within circuit as distinct list
def toCircuitCoordinatesList(c):
    listCoords = []
    for i, el in c:
        listCoords.append(i)
        listCoords.append(el)
    return list(set(listCoords))

def circuitComparator(c):
    return -len(c)

def calculateDistance(c1, c2):
    return sqrt((c1[0] - c2[0])**2 + (c1[1] - c2[1])**2 + (c1[2] - c2[2])**2)

def distanceComparator(d):
    return d[2]
