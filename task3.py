import path

def run():
    sum = 0
    with open(path.path) as text:
        for line in text:
            digits = list(map(int, line.strip()))
            maxDigit = max(digits)
            indexMaxDigit = digits.index(maxDigit)
            # If max digit is the last one: look for the second before
            if indexMaxDigit == len(digits) -1:
                maxSecondDigit = max(digits[0:-1])
                sum += int(str(maxSecondDigit) + str(maxDigit))
            else: # Look for the second max digit befor the first
                maxSecondDigit = max(digits[indexMaxDigit+1:])
                sum += int(str(maxDigit) + str(maxSecondDigit))
    print(sum)

def run2():
    sum = 0
    with open(path.path) as text:
        for line in text:
            lineStripped = line.strip()
            digits = list(map(int, lineStripped))
            lineRes = ""
            # Check each joltages first occurence and the remaining positions afterwards. Take the highest joltage with sufficient remaining positions
            for i in range(11, -1, -1):
                digits = list(map(int, lineStripped))
                for joltage in range(9,0, -1):
                    if joltage not in digits:
                        continue
                    maxDigitIndex = digits.index(joltage)
                    if len(lineStripped) - maxDigitIndex -1 >= i:
                        lineRes += str(joltage)
                        lineStripped = lineStripped[maxDigitIndex + 1:]
                        break
            sum += int(lineRes)
    print(sum)
