/**
 * Thoughts:
 * Think of needed indicators and buttom schemas as equations to be solved.
 * Looking athe the input, I realized the problem would be an overdetermined system of equations which would be difficult to solve for least amount of button clicks.
 * There is an approach working with inequalities and maximize using a target function.
 * The apporach is called Simplex Algorithm and can be adapted for this problem:
 * Think about an equation like a + b = 1 as two inequalities: a + b <= 1 AND a + b >= 1
 * Each button schema represent a structure variable of the problem with its values reprent the values for the latter constraint. 
 * Example (a, b, c and d only represent the index of the indicators (for readability) and are not requirement in the problem): 
 * * (1,2,4) results in: x1 b + x1 c + x1 d >= 0 AND
 * * (0,1,3,4) results in: x2 a + x2 b x2 d + x2 e >= 0
 * Let's say the indicators look like this: {2,4,6,8,10}
 * The resulting constraints would be:
 * * 0x1 + 1x2 + ... = 2
 * * 1x1 + 1x2 + ... = 4
 * * 1x1 + 0x2 + ... = 6 and so on
 * Using this problem definition, we are able to solve the problem optimally if we can solve the problem that Simplex only maximizes.
 * This can be achieved if you think about minimizing the target function F is the same as to maximize -F.
 * 
 * However some of the solutions will contain non integer values for the structur variables (and for the solution value as well). 
 * It is not possible to do rounding and the Simplex algorithm is not capable of handling such a constraint directly. 
 * We can add constraints when finding an optimal solution that contains non integer using cuts like explained here: https://en.wikipedia.org/wiki/Cutting-plane_method (espaciialy Gomory cuts)
 * The idea is to add a constraint to prevent the algorithm of using a non integer solution by cutting the found "optimal" out.
 * This produces an invalid solution (thus resulting in the usage of Dual Simplex or M Method) and a new valid and optimal solution is to be found.
 * This process is repeated until the otimal solution only contains integer values.
 * 
 * 
 * As I already implemented several versions of Simplex algorithms (Primal, Dual, M Method as well as Duality), my motivation was low.
 * So I decided to find an existing solver. 
 * 
 * Feel free to ask for my already implemented software so I can send you the link.
 * 
 * 
 * Credits for the used library: https://github.com/JWally/jsLPSolver (Unlicense license)
 */


let buttonClickCount = 0;

require('fs').readFileSync('input.txt', 'utf-8').split(/\r?\n/).forEach(function(line)
{
    let splittedRes = line.split("]")[1].split("{");
    let buttonSchemata = splittedRes[0].trim().split(" ")
    let indicators = splittedRes[1].replace("}", "").split(",")

    // Build instance for solver
    var model = {
        "optimize": "F", // Target function. Each variable will contain the F attribute with value 1 as hitting a button will increase the target value by 1.
        "opType": "min",
        "constraints": {
        },
        "variables": {},
        "ints" : {}
    };

    // Fill constraints
    for (i in indicators)
    {
        model.constraints["indicator" + i] = {
            equal: parseInt(indicators[i], 10)
        }
    }

    // Fill variables and set 'only integer solutions' condition 
    for (i in buttonSchemata)
    {
        model.ints["x" + i] = 1; // Only integer


        model.variables["x" + i] = {
            F: 1 // For target function
        }

        let currentSchema = buttonSchemata[i];
        currentSchema = currentSchema.replace("(", "").replace(")", "")
        let buttonIndicators = currentSchema.split(",");
        for(j in buttonIndicators)
        {
            model.variables["x" + i]["indicator" + parseInt(buttonIndicators[j])] = 1
        }
    }

    var solver = require("./node_modules/javascript-lp-solver/src/solver"),
    results;

    results = solver.Solve(model)
    buttonClickCount += results.result
})

console.log("Ergebnis: " + buttonClickCount)
