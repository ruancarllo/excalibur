namespace ExcaliburUI {
  public class ExcaliburWindow : System.Windows.Forms.Form {
    public readonly CopyButton CopyButton_X;
    public readonly CopyButton CopyButton_Y;
    public readonly CopyButton CopyButton_Z;

    public readonly ActualDimensionTextBox ActualDimensionTextBox_Width;
    public readonly ActualDimensionTextBox ActualDimensionTextBox_Height;
    public readonly ActualDimensionTextBox ActualDimensionTextBox_Depth;

    public readonly DilationNameLabel DilationNameLabel_X;
    public readonly DilationNameLabel DilationNameLabel_Y;
    public readonly DilationNameLabel DilationNameLabel_Z;

    public readonly DilationFactorTextBox DilationFactorTextBox_X;
    public readonly DilationFactorTextBox DilationFactorTextBox_Y;
    public readonly DilationFactorTextBox DilationFactorTextBox_Z;

    public readonly ModeCheckBox ModeCheckBox_Percentual;
    public readonly ModeNameLabel ModeNameLabel_Percentual;

    public readonly GreatButton UndoButton;
    public readonly GreatButton ApplyButton;

    public readonly SmallButton SubdilateButton;
    public readonly SmallButton SuperdilateButton;

    public ExcaliburWindow() {
      Text = "Excalibur";

      AutoSize = true;
      AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

      Padding = new System.Windows.Forms.Padding(Sizes.WindowPadding);

      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

      TopMost = true;

      Font = new System.Drawing.Font("Arial", Sizes.FontSize, System.Drawing.FontStyle.Bold);

      CopyButton_X = new CopyButton(0);
      CopyButton_Y = new CopyButton(1);
      CopyButton_Z = new CopyButton(2);

      ActualDimensionTextBox_Width = new ActualDimensionTextBox(0);
      ActualDimensionTextBox_Height = new ActualDimensionTextBox(1);
      ActualDimensionTextBox_Depth = new ActualDimensionTextBox(2);

      DilationNameLabel_X = new DilationNameLabel(0, "X");
      DilationNameLabel_Y = new DilationNameLabel(1, "Y");
      DilationNameLabel_Z = new DilationNameLabel(2, "Z");

      DilationFactorTextBox_X = new DilationFactorTextBox(0);
      DilationFactorTextBox_Y = new DilationFactorTextBox(1);
      DilationFactorTextBox_Z = new DilationFactorTextBox(2);

      ModeCheckBox_Percentual = new ModeCheckBox();
      ModeNameLabel_Percentual = new ModeNameLabel();

      UndoButton = new GreatButton(0, "Desfazer");
      ApplyButton = new GreatButton(1, "Aplicar");

      SubdilateButton = new SmallButton(0, "-");
      SuperdilateButton = new SmallButton(1, "+");

      Controls.Add(CopyButton_X);
      Controls.Add(CopyButton_Y);
      Controls.Add(CopyButton_Z);

      Controls.Add(ActualDimensionTextBox_Width);
      Controls.Add(ActualDimensionTextBox_Height);
      Controls.Add(ActualDimensionTextBox_Depth);

      Controls.Add(DilationNameLabel_X);
      Controls.Add(DilationNameLabel_Y);
      Controls.Add(DilationNameLabel_Z);

      Controls.Add(DilationFactorTextBox_X);
      Controls.Add(DilationFactorTextBox_Y);
      Controls.Add(DilationFactorTextBox_Z);

      Controls.Add(ModeCheckBox_Percentual);
      Controls.Add(ModeNameLabel_Percentual);

      Controls.Add(UndoButton);
      Controls.Add(ApplyButton);

      Controls.Add(SubdilateButton);
      Controls.Add(SuperdilateButton);
    }
  }

  public class CopyButton : System.Windows.Forms.Button {
    public CopyButton(int row) {
      Size = new System.Drawing.Size(Sizes.CopyButtonWidth, Sizes.CopyButtonHeight);

      FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      FlatAppearance.BorderSize = 0;
      
      Location = new System.Drawing.Point(
        Sizes.WindowPadding,
        Sizes.WindowPadding + Sizes.WindowPadding * row + Sizes.CopyButtonHeight * row
      );

      BackColor = System.Drawing.Color.FromArgb(0, 127, 0);
    }
  }

  public class ActualDimensionTextBox : System.Windows.Forms.TextBox {
    public ActualDimensionTextBox(int row) {
      MinimumSize = new System.Drawing.Size(Sizes.ActualDimensionTextBoxWidth, Sizes.ActualDimensionTextBoxHeight);
      MaximumSize = new System.Drawing.Size(Sizes.ActualDimensionTextBoxWidth, Sizes.ActualDimensionTextBoxHeight);

      Location = new System.Drawing.Point(
        2 * Sizes.WindowPadding + Sizes.CopyButtonWidth,
        Sizes.WindowPadding + Sizes.WindowPadding * row + Sizes.ActualDimensionTextBoxHeight * row
      );

      ReadOnly = true;

      BackColor = base.BackColor;
      ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
    }
  }

  public class DilationNameLabel : System.Windows.Forms.Label {
    public DilationNameLabel(int row, string name) {
      Size = new System.Drawing.Size(Sizes.DilationNameLabelWidth, Sizes.DilationNameLabelHeight);

      Location = new System.Drawing.Point(
        3 * Sizes.WindowPadding + Sizes.CopyButtonWidth + Sizes.ActualDimensionTextBoxWidth,
        Sizes.WindowPadding + Sizes.WindowPadding * row + Sizes.DilationFactorTextBoxHeight * row
      );

      TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      
      Text = $"Escala de {name}";
    }
  }

  public class DilationFactorTextBox : System.Windows.Forms.TextBox {
    public DilationFactorTextBox(int row) {
      MinimumSize = new System.Drawing.Size(Sizes.DilationFactorTextBoxWidth, Sizes.DilationFactorTextBoxHeight);
      MaximumSize = new System.Drawing.Size(Sizes.DilationFactorTextBoxWidth, Sizes.DilationFactorTextBoxHeight);

      Location = new System.Drawing.Point(
        4 * Sizes.WindowPadding + Sizes.CopyButtonWidth + Sizes.DilationFactorTextBoxWidth + Sizes.DilationNameLabelWidth,
        Sizes.WindowPadding + Sizes.WindowPadding * row + Sizes.DilationFactorTextBoxHeight * row
      );
    }
  }

  public class ModeCheckBox : System.Windows.Forms.CheckBox {
    public ModeCheckBox() {
      Size = new System.Drawing.Size(Sizes.ModeCheckBoxWidth, Sizes.ModeCheckBoxHeight);

      Location = new System.Drawing.Point(
        Sizes.WindowPadding,
        4 * Sizes.WindowPadding + 3 * Sizes.CopyButtonHeight + Sizes.WindowPadding / 2
      );
    }
  }

  public class ModeNameLabel : System.Windows.Forms.Label {
    public ModeNameLabel() {
      Size = new System.Drawing.Size(Sizes.ModeNameLabelWidth, Sizes.ModeNameLabelHeight);

      Location = new System.Drawing.Point(
        2 * Sizes.WindowPadding + Sizes.ModeCheckBoxWidth,
        4 * Sizes.WindowPadding + 3 * Sizes.CopyButtonHeight
      );

      TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

      Text = "Contração (%)";
    }
  }

  public class GreatButton : System.Windows.Forms.Button {
    public GreatButton(int column, string name) {
      Size = new System.Drawing.Size(Sizes.GreatButtonWidth, Sizes.GreatButtonHeight);

      Location = new System.Drawing.Point(
        3 * Sizes.WindowPadding + Sizes.ModeCheckBoxWidth + Sizes.ModeNameLabelWidth + (Sizes.GreatButtonWidth + Sizes.WindowPadding) * column,
        4 * Sizes.WindowPadding + 3 * Sizes.CopyButtonHeight
      );

      Text = name;
    }
  }

  public class SmallButton : System.Windows.Forms.Button {
    public SmallButton(int column, string name) {
      Size = new System.Drawing.Size(Sizes.SmallButtonWidth, Sizes.SmallButtonHeight);

      Location = new System.Drawing.Point(
        5 * Sizes.WindowPadding + Sizes.ModeCheckBoxWidth + Sizes.ModeNameLabelWidth + 2 * Sizes.GreatButtonWidth + (Sizes.SmallButtonWidth + Sizes.WindowPadding) * column,
        4 * Sizes.WindowPadding + 3 * Sizes.CopyButtonHeight
      );

      Text = name;
    }
  }

  public class Sizes {
    public static readonly int ScaleFactor = 15;

    public static readonly int CopyButtonWidth = 2 * ScaleFactor;
    public static readonly int CopyButtonHeight = 3 * ScaleFactor;

    public static readonly int ActualDimensionTextBoxWidth = 16 * ScaleFactor;
    public static readonly int ActualDimensionTextBoxHeight = 3 * ScaleFactor;

    public static readonly int DilationNameLabelWidth = 12 * ScaleFactor;
    public static readonly int DilationNameLabelHeight = 3 * ScaleFactor;

    public static readonly int DilationFactorTextBoxWidth = 16 * ScaleFactor;
    public static readonly int DilationFactorTextBoxHeight = 3 * ScaleFactor;

    public static readonly int ModeCheckBoxWidth = 2 * ScaleFactor;
    public static readonly int ModeCheckBoxHeight = 2 * ScaleFactor;

    public static readonly int ModeNameLabelWidth = 14 * ScaleFactor;
    public static readonly int ModeNameLabelHeight = 3 * ScaleFactor;

    public static readonly int GreatButtonWidth = 11 * ScaleFactor;
    public static readonly int GreatButtonHeight = 3 * ScaleFactor;

    public static readonly int SmallButtonWidth = 3 * ScaleFactor;
    public static readonly int SmallButtonHeight = 3 * ScaleFactor;

    public static readonly int WindowPadding = 1 * ScaleFactor;

    public static readonly int FontSize = 10;
  }
}