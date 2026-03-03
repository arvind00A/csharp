
### Operators
 
Operators are special symbols or keywords that perform operations on one or more operands (variables, literals, expressions).

#### Arithmetic Operators

| Operator | Name              | Example      | Result | Notes                                   |
|----------|-------------------|--------------|--------|-----------------------------------------|
| `+`      | Addition          | `5 + 3`      | 8      | Also string concatenation               |
| `-`      | Subtraction       | `10 - 4`     | 6      | Unary minus: `-x`                       |
| `*`      | Multiplication    | `6 * 7`      | 42     | —                                       |
| `/`      | Division          | `10 / 3`     | 3      | Integer division truncates toward zero  |
| `%`      | Modulus           | `10 % 3`     | 1      | Remainder                               |

**Compound assignment** (shorthand): `+=` `-=` `*=` `/=` `%=`

#### Relational Operators (return `bool`)

| Operator | Meaning                   | Example     | Result  |
|----------|---------------------------|-------------|---------|
| `==`     | Equal                     | `5 == 5`    | `true`  |
| `!=`     | Not equal                 | `5 != 3`    | `true`  |
| `<`      | Less than                 | `4 < 10`    | `true`  |
| `>`      | Greater than              | `15 > 7`    | `true`  |
| `<=`     | Less than or equal        | `5 <= 5`    | `true`  |
| `>=`     | Greater than or equal     | `8 >= 10`   | `false` |

#### Logical Operators (work on `bool`)

| Operator | Name     | Example                | Result when...                          | Short-circuit? |
|----------|----------|------------------------|------------------------------------------|----------------|
| `&&`     | AND      | `true && false`        | `true` only if **both** are true         | Yes            |
| `||`     | OR       | `true || false`        | `true` if **at least one** is true       | Yes            |
| `!`      | NOT      | `!true`                | Reverses the value                       | —              |

**Example combining operators**

```csharp
int a = 17, b = 5;
bool isBig = a > 10 && (a % 2 == 1);          // true
bool safeDivide = b != 0 && (a / b > 3);      // short-circuit prevents /0

```
 **Operator Precedence**
1. ! (not)
2. \* / % (multiplication, division, modulus)
3. \+ - (addition, subtraction)
4. < > <= >= (relational and type testing)
5. == != (equality and inequality)
6. && (logical AND)
7. || (logical OR)
