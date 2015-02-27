# Quants

Created by Lars Moastuen (lars@moastuen.com).
Licensed under the MIT license (see LICENSE).

## Introduction
This library implements basic unit support for .NET. The 
library has been inspired by Boost.Units and the native unit support in
F#, but differs in a few key aspect (see list below). The goal of 
the library is to provide users arthimetic operations that maintain 
the units, making it a lot easier to detect bugs in mathematical 
computations involving physical phenomena.

In the current state the library is more a proof of concept (or
possibly a good argument why one would not want to do something like this)
on how "unit aware quantities" could be implemented in .NET.

The library consists of three important aspects/concepts that form
a unit system.

## Dimensions
Dimensions are "unit categories". Dimensions describe what a
quantity measures, but not how. "Length" and "Mass" are examples
of dimensions. Dimensions can be combined to form complex 
dimensions, such as Area = Length**Length or 
Speed = Length/Time.

The base interface for dimensions is IDimension. New dimensions
are typically created from the BaseDimension-implementation. There 
is one special dimension, "dimensionless", that is defined by the 
Dimensionless-class.

## Units
Units are bound to one dimension and describes how a physical 
quantity is stored. Examples of units for "Length" are "meter" 
and "feet" while "kg" and "lbs" are typical units for the "Mass"
dimension. Units can be combined to form complex units of complex
dimensions, such as liter=dm**dm**dm in the 
Volume=Length**Length**Length dimension.

Value converters are provided to be able to convert between units 
within the same dimension.

The base interface for units is IUnit. New units are typically created from
the BaseUnit-implementation. There is one special unit, "unitless", defined 
in the Unitless-class.

## Quantities
Quantities are measurements with unit, e.g. "1.0 kg". They support basic
arithmetic operations such as addition, subtraction, multiplication and 
division.

Note that care should be taken when combining (through arithmetic) quantities
with different units. It is e.g. perfectly legal and possible to combine quanties
of different units, but equal dimensions (e.g. "meter" and "feet"). Computations such as
"5.0 m ** 0.5 ft" are one example of this. Although perfectly valid, the framework
currently does not (out of the box) support converting this to e.g. "m^2".

Adding or subtracting quantities of different units are naturally not support - even
if the dimensions are the same. This would require unit conversions.

The framework comes with a few predefined quantities:
Quantity<double> - double quantity
Quantity<float> - float quantity
Quantity<Matrix<double>> - matrix quantity (through MathNet)
Quantity<Vector<double>> - vector quantity (through MathNet)

Note that it is usually best to avoid using simple types such as QuantityD and QuantityF
because there a huge overhead in performing arithmetic on unit and dimension types. Therefore,
matrices and vectors should be used where possible. If not possible you should consider using
regular arithmitics on the values and manually create units for the result, e.g.:

    p = v*t + (1/2)*a*t*t

If you know that the units of v is "m/s", t is "s" and a is "m/(s**s)", it's clear that
the resulting unit will be "m". If only the dimensions are known up front, not units, some
optimization could be gained by only determining the units from one of the terms, e.g. "v**t".
This will work as it is required that quantities have the same unit when adding and substracting.

In addition to the three concepts above (Dimensions, Units and Quantities) there's a few
classes that form a complete unit system/unit catalogue.

## Unit systems
The unit system is responsible for maintaining a set of dimensions and quantities and
provide means of converting between them. Unit systems are completly customizable 
and it is perfectly possible to implement an unit system by implementing the 
IUnitSystem-interface or by using the predefined UnitSystem class that supports building 
an unit system run-time. The UnitSystem class supports tree-like relations between units, e.g.
"kg" can be the base unit for "Mass" and "g" can be a sub-unit of "kg". Other "Mass" units
such as "lbs" could be subunits of "kg" or "g" - the system will be able to convert between
all of these by converting values through the necessary steps.

In many cases it is sufficient to use the predefined SI unit system defined in
class UnitSystemSI. It is possible to extend this unit system with custom dimensions
and units.

## Value converters
Value converters are responsible for converting from one unit to another (within the
same dimension). Support is dodgy at best.


## Example
See BasicExample.csproj.


## Future work
- Its not possible to perform arithmetic operations using quantities with the
  same dimension, but different units. An example of this is "1 m ** 2 cm" where
  one would expect "1 m ** 0.01 m = 0.01 m^2". Another example is "1 m^3 - 1000 dm^3"
  where one would expect "0 m^3".
  
- Add support for custom symbols and name for derived dimensions. Currently
  will e.g. the Area dimension be denoted as Length * Length.
  
- Add support for custom unit and description for derived units. Currently
  will e.g. the "liter" unit be denoted as "dm*dm*dm".
  
- Make it easier to combine units to form devised units (e.g. density is kg / m^3).

- Extend IUnit and IDimension to enable the units to look up equivalent units/dimensions.
  Add conversion on IUnit
  
- Better support for casting quantities, e.g. casting Quantity<float> to Quantity<double>.

## Differences between Boost.Units and this library
Besides from Boost.Units being a more elaborate library, there are some key restrictions
to .NET that is important to be aware of:

- The library has runtime overhead. This is because there is no compile time 
  meta-programming capabilities in .NET/C#.
  
- Does not support compile time units checking, analysis or calculation.
  This is also because there is no meta-programming capatibilies in
  .NET/C#.
  
- Quantities support generics data types, but because of the nature of
  generics the quantity class must be overriden for each new datatype
  to support. This is because there is no way to enforce generic types
  to be multiplicable, dividiable etc.