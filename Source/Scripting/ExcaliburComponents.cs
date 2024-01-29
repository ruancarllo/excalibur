namespace ExcaliburComponents {
  public class MainWindow: System.Windows.Forms.Form {
    public MainWindow() {
      Text = "Excalibur";

      TopMost = true;
      
      AutoSize = true;
      AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

      StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen; 

      FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

      Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);

      Padding = new System.Windows.Forms.Padding(1 * ExcaliburComponents.SizeProportions.ScaleFactor);

      Controls.Add(ColorButtonForX);
      Controls.Add(ColorButtonForY);
      Controls.Add(ColorButtonForZ);

      Controls.Add(ReadOnlyTextBoxForWidth);
      Controls.Add(ReadOnlyTextBoxForHeight);
      Controls.Add(ReadOnlyTextBoxForDepth);
      
      Controls.Add(SmallLabelForX);
      Controls.Add(SmallLabelForY);
      Controls.Add(SmallLabelForZ);

      Controls.Add(MediumWritableTextBoxForX);
      Controls.Add(MediumWritableTextBoxForY);
      Controls.Add(MediumWritableTextBoxForZ);

      Controls.Add(CheckBoxForPercentual);
      Controls.Add(LargeLabelForPercentual);

      Controls.Add(MediumButtonForUndo);
      Controls.Add(MediumButtonForApply);

      Controls.Add(SmallButtonForSubdilate);
      Controls.Add(SmallButtonForSuperdilate);

      Controls.Add(MediumLabelForReference);
      Controls.Add(SmallWritableTextBoxForReference);

      Controls.Add(HighWritableTextBoxForValues);

      Controls.Add(LargeButtonForSpread);
    }

    public readonly ExcaliburComponents.ColorButton ColorButtonForX = new ExcaliburComponents.ColorButton {
      Location = new System.Drawing.Point(1 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.ColorButton ColorButtonForY = new ExcaliburComponents.ColorButton {
      Location = new System.Drawing.Point(1 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.ColorButton ColorButtonForZ = new ExcaliburComponents.ColorButton {
      Location = new System.Drawing.Point(1 * ExcaliburComponents.SizeProportions.ScaleFactor, 9 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.ReadOnlyTextBox ReadOnlyTextBoxForWidth = new ExcaliburComponents.ReadOnlyTextBox {
      Location = new System.Drawing.Point(4 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.ReadOnlyTextBox ReadOnlyTextBoxForHeight = new ExcaliburComponents.ReadOnlyTextBox {
      Location = new System.Drawing.Point(4 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.ReadOnlyTextBox ReadOnlyTextBoxForDepth = new ExcaliburComponents.ReadOnlyTextBox {
      Location = new System.Drawing.Point(4 * ExcaliburComponents.SizeProportions.ScaleFactor, 9 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.SmallLabel SmallLabelForX = new ExcaliburComponents.SmallLabel {
      Location = new System.Drawing.Point(21 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Escala de X",
    };

    public readonly ExcaliburComponents.SmallLabel SmallLabelForY = new ExcaliburComponents.SmallLabel {
      Location = new System.Drawing.Point(21 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Escala de Y",
    };

    public readonly ExcaliburComponents.SmallLabel SmallLabelForZ = new ExcaliburComponents.SmallLabel {
      Location = new System.Drawing.Point(21 * ExcaliburComponents.SizeProportions.ScaleFactor, 9 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Escala de Z",
    };

    public readonly ExcaliburComponents.MediumWritableTextBox MediumWritableTextBoxForX = new ExcaliburComponents.MediumWritableTextBox {
      Location = new System.Drawing.Point(31 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.MediumWritableTextBox MediumWritableTextBoxForY = new ExcaliburComponents.MediumWritableTextBox {
      Location = new System.Drawing.Point(31 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.MediumWritableTextBox MediumWritableTextBoxForZ = new ExcaliburComponents.MediumWritableTextBox {
      Location = new System.Drawing.Point(31 * ExcaliburComponents.SizeProportions.ScaleFactor, 9 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.CheckBox CheckBoxForPercentual = new ExcaliburComponents.CheckBox {
      Location = new System.Drawing.Point(1 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor + 1 * ExcaliburComponents.SizeProportions.CompletionFactor),
    };

    public readonly ExcaliburComponents.LargeLabel LargeLabelForPercentual = new ExcaliburComponents.LargeLabel {
      Location = new System.Drawing.Point(4 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Contração (%)",
    };

    public readonly ExcaliburComponents.MediumButton MediumButtonForUndo = new ExcaliburComponents.MediumButton {
      Location = new System.Drawing.Point(16 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Desfazer",
    };

    public readonly ExcaliburComponents.MediumButton MediumButtonForApply = new ExcaliburComponents.MediumButton {
      Location = new System.Drawing.Point(28 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Aplicar"
    };

    public readonly ExcaliburComponents.SmallButton SmallButtonForSubdilate = new ExcaliburComponents.SmallButton {
      Location = new System.Drawing.Point(40 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "-",
    };
    
    public readonly ExcaliburComponents.SmallButton SmallButtonForSuperdilate = new ExcaliburComponents.SmallButton {
      Location = new System.Drawing.Point(44 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "+",
    };

    public readonly ExcaliburComponents.MediumLabel MediumLabelForReference = new ExcaliburComponents.MediumLabel {
      Location = new System.Drawing.Point(48 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Nº de referência"
    };

    public readonly ExcaliburComponents.SmallWritableTextBox SmallWritableTextBoxForReference = new ExcaliburComponents.SmallWritableTextBox {
      Location = new System.Drawing.Point(60 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.HighWritableTextBox HighWritableTextBoxForValues = new ExcaliburComponents.HighWritableTextBox {
      Location = new System.Drawing.Point(48 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.LargeButton LargeButtonForSpread = new ExcaliburComponents.LargeButton {
      Location = new System.Drawing.Point(48 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Projetar numeração",
    };
  }

  public class ReadOnlyTextBox: System.Windows.Forms.TextBox {
    public ReadOnlyTextBox() {
      MinimumSize = new System.Drawing.Size(16 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
      MaximumSize = new System.Drawing.Size(16 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);

      ReadOnly = true;

      BackColor = base.BackColor;
      ForeColor = System.Drawing.Color.FromArgb(0, 0, 255);
    }
  }

  public class SmallWritableTextBox: System.Windows.Forms.TextBox {
    public SmallWritableTextBox() {
      MinimumSize = new System.Drawing.Size(3 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
      MaximumSize = new System.Drawing.Size(3 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);

      TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
    }
  }

  public class MediumWritableTextBox: System.Windows.Forms.TextBox {
    public MediumWritableTextBox() {
      MinimumSize = new System.Drawing.Size(16 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
      MaximumSize = new System.Drawing.Size(16 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
    }
  }

  public class HighWritableTextBox: System.Windows.Forms.TextBox {
    public HighWritableTextBox() {
      MinimumSize = new System.Drawing.Size(15 * ExcaliburComponents.SizeProportions.ScaleFactor, 7 * ExcaliburComponents.SizeProportions.ScaleFactor);
      MaximumSize = new System.Drawing.Size(15 * ExcaliburComponents.SizeProportions.ScaleFactor, 7 * ExcaliburComponents.SizeProportions.ScaleFactor);

      Multiline = true;
    }
  }

  public class SmallLabel: System.Windows.Forms.Label {
    public SmallLabel() {
      Size = new System.Drawing.Size(9 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
      
      TextAlign = System.Drawing.ContentAlignment.MiddleRight;
    }
  }

  public class MediumLabel: System.Windows.Forms.Label {
    public MediumLabel() {
      Size = new System.Drawing.Size(11 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
      
      TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
    }
  }

  public class LargeLabel: System.Windows.Forms.Label {
    public LargeLabel() {
      Size = new System.Drawing.Size(11 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);

      TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
    }
  }

  public class ColorButton: System.Windows.Forms.Button {
    public ColorButton() {
      Size = new System.Drawing.Size(2 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);

      FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      FlatAppearance.BorderSize = 0;

      BackColor = System.Drawing.Color.FromArgb(0, 127, 0);
    }
  }

  public class SmallButton: System.Windows.Forms.Button {
    public SmallButton() {
      Size = new System.Drawing.Size(3 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
    }
  }

  public class MediumButton: System.Windows.Forms.Button {
    public MediumButton() {
      Size = new System.Drawing.Size(11 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
    }
  }

  public class LargeButton: System.Windows.Forms.Button {
    public LargeButton() {
      Size = new System.Drawing.Size(15 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
    }
  }

  public class CheckBox: System.Windows.Forms.CheckBox {
    public CheckBox() {
      Size = new System.Drawing.Size(2 * ExcaliburComponents.SizeProportions.ScaleFactor, 2 * ExcaliburComponents.SizeProportions.ScaleFactor);
    }
  }

  public class SizeProportions {
    public static System.Int32 ScaleFactor = 15;
    public static System.Int32 CompletionFactor = 8;
  }
}