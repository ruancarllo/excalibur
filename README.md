# About the  Excalibur software

This software is a plugin for the three-dimensional modeling program [Rhinoceros](https://www.rhino3d.com/), capable of providing a simple user interface for scaling three-dimensional objects in addition, contraction and arithmetic progression, following a custom theorem described below.

## How to build this plugin

To build this plugin in a **.rhp** file extension, install the [.NET Framework](https://dotnet.microsoft.com/) in your computer, open this project root folder with your shell, and run:

```sh
dotnet build
```

## The Ruan Carllo's three-dimensional progressive scale conjecture

Given a 4x1 matrix that represents any $\vec{P}$ vectorial point of an object with dimensions $W_0$, $H_0$ and $D_0$ and center with coordinates $C_x$, $C_y$ and $C_z$, whose dimensions in the axes $x$, $y$ and $z$ must be increased, respectively, by $I_x$, $I_y$ and $I_z$ units, with a spacing of $G_x$, $G_y$ and $G_z$ units between each scaled object of number $n$, the resulting vectorial point $\vec{P'}$ is equal to:

$$
\vec{P'} = E \times \vec{P}
$$

In such a way that $E$ represents the operator 4x4 matrix at the point $\vec{P}$, defined as:

$$
E =
\begin{bmatrix}
S_x & 0 & 0 & C_x \times (1 - S_x) + T_x \\
0 & S_y & 0 & C_y \times (1 - S_y) + T_y \\
0 & 0 & S_z & C_z \times (1 - S_z) + T_z \\
0 & 0 & 0 & 1
\end{bmatrix}
$$

Where $S_x$, $S_y$ and $S_z$ represent the multiplicative scale factors of the object on the $x$, $y$ and $z$ axes, being equal to:

$$
\begin{align*}
S_x &= \frac{W_0 \pm I_x \times n}{W_0} \\
S_y &= \frac{H_0 \pm I_y \times n}{H_0} \\
S_z &= \frac{D_0 \pm I_z \times n}{D_0}
\end{align*}
$$

And where $T_x$, $T_y$ and $T_z$ represent the linear translation factors for each of the $x$, $y$ and $z$ axes, defined as:

$$
\begin{align*}
T_x = I_x \times \frac{n^2 + n}{2} \pm (W_0  + G_x) \times n \\
T_y = I_y \times \frac{n^2 + n}{2} \pm (H_0  + G_y) \times n \\
T_z = I_z \times \frac{n^2 + n}{2} \pm (D_0  + G_z) \times n
\end{align*}
$$

So the plus or minus sign indicates whether objects are being scaled up or down.