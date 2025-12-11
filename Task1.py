import path

# First
def run():
    count = 0
    start = 50
    with open(path.path) as f:
        for x in f:
            start = start + int(x[1:]) if x[0] == "R" else start - int(x[1:])
            if start % 100 == 0:
                count += 1

    print("Anzahl der Nullen: " + str(count))

# Second lazy: Add each click in a loop and check for ots modulo value (easy to programm, not very efficient)
def run2Lazy():
    count = 0
    start = 50
    with open(path.path) as f:
        for x in f:
            for i in range(1, int(x[1:]) + 1):
                start = start + 1 if x[0] == "R" else start - 1
                if start % 100 == 0:
                    count += 1

    print("Anzahl der Nullen: " + str(count))

# Second efficient
def run2():
    count = 0
    start = 50
    with open(path.path) as f:
        for x in f:
            oldStart = start
            modRes = int(x[1:]) % 100
            start = start + modRes if x[0] == "R" else start - modRes
            count += int(int(x[1:]) / 100)
            if oldStart != 0 and start == 0 or oldStart > 0 and start < 0 or oldStart < 0 and start > 0:
                count += 1
            if start != 0:
                count += int(start / 100)
            start = start % 100

    print("Anzahl der Nullen: " + str(count))
