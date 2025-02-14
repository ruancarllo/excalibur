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

      Controls.Add(FlagButtonForBrazil);
      Controls.Add(FlagButtonForUnitedStates);

      Controls.Add(MediumLabelForReference);
      Controls.Add(SmallWritableTextBoxForReference);

      Controls.Add(HighWritableTextBoxForValuesColumn1);
      Controls.Add(HighWritableTextBoxForValuesColumn2);
      Controls.Add(HighWritableTextBoxForValuesColumn3);
      Controls.Add(HighWritableTextBoxForValuesColumn4);

      Controls.Add(LargeButtonForSpread);

      HighWritableTextBoxForValuesColumn1.KeyDown += HandleEnterKeyNavigation;
      HighWritableTextBoxForValuesColumn2.KeyDown += HandleEnterKeyNavigation;
      HighWritableTextBoxForValuesColumn3.KeyDown += HandleEnterKeyNavigation;
      HighWritableTextBoxForValuesColumn4.KeyDown += HandleEnterKeyNavigation;
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

    public readonly ExcaliburComponents.FlagButton FlagButtonForBrazil = new ExcaliburComponents.FlagButton {
      Location = new System.Drawing.Point(48 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
      BackgroundImage = System.Drawing.Image.FromFile(
        System.IO.Path.Combine(
          ExcaliburAdministration.ExternalPlugIn.AssemblyPath,
          "br-flag.png"
        )
      )
    };

    public readonly ExcaliburComponents.FlagButton FlagButtonForUnitedStates = new ExcaliburComponents.FlagButton {
      Location = new System.Drawing.Point(48 * ExcaliburComponents.SizeProportions.ScaleFactor, 4 * ExcaliburComponents.SizeProportions.ScaleFactor),
      BackgroundImage = System.Drawing.Image.FromFile(
        System.IO.Path.Combine(
          ExcaliburAdministration.ExternalPlugIn.AssemblyPath,
          "us-flag.png"
        )
      )
    };

    public readonly ExcaliburComponents.MediumLabel MediumLabelForReference = new ExcaliburComponents.MediumLabel {
      Location = new System.Drawing.Point(52 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Nº de referência"
    };

    public readonly ExcaliburComponents.SmallWritableTextBox SmallWritableTextBoxForReference = new ExcaliburComponents.SmallWritableTextBox {
      Location = new System.Drawing.Point(65 * ExcaliburComponents.SizeProportions.ScaleFactor, 1 * ExcaliburComponents.SizeProportions.ScaleFactor),
    };

    public readonly ExcaliburComponents.HighWritableTextBoxColumn HighWritableTextBoxForValuesColumn1 = new ExcaliburComponents.HighWritableTextBoxColumn {
      Location = new System.Drawing.Point(52 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Name = "HighWritableTextBoxForValuesColumn1"
    };

    public readonly ExcaliburComponents.HighWritableTextBoxColumn HighWritableTextBoxForValuesColumn2 = new ExcaliburComponents.HighWritableTextBoxColumn {
      Location = new System.Drawing.Point(56 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Name = "HighWritableTextBoxForValuesColumn2"
    };

    public readonly ExcaliburComponents.HighWritableTextBoxColumn HighWritableTextBoxForValuesColumn3 = new ExcaliburComponents.HighWritableTextBoxColumn {
      Location = new System.Drawing.Point(60 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Name = "HighWritableTextBoxForValuesColumn3"
    };

    public readonly ExcaliburComponents.HighWritableTextBoxColumn HighWritableTextBoxForValuesColumn4 = new ExcaliburComponents.HighWritableTextBoxColumn {
      Location = new System.Drawing.Point(64 * ExcaliburComponents.SizeProportions.ScaleFactor, 5 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Name = "HighWritableTextBoxForValuesColumn4"
    };

    public readonly ExcaliburComponents.LargeButton LargeButtonForSpread = new ExcaliburComponents.LargeButton {
      Location = new System.Drawing.Point(52 * ExcaliburComponents.SizeProportions.ScaleFactor, 13 * ExcaliburComponents.SizeProportions.ScaleFactor),
      Text = "Projetar numeração",
    };

    private void HandleEnterKeyNavigation(object sender, System.Windows.Forms.KeyEventArgs e) {
      if (e.KeyCode == System.Windows.Forms.Keys.Enter) {
        System.Windows.Forms.TextBox currentTextBox = sender as System.Windows.Forms.TextBox;

        if (currentTextBox != null) {
          System.String[] currentTextBoxLines = currentTextBox.Text.Trim().Split(
            new System.String[] { System.Environment.NewLine },
            System.StringSplitOptions.None
          );

          if (currentTextBoxLines.Length >= 4) {
            e.Handled = true;
            e.SuppressKeyPress = true;

            switch (currentTextBox.Name) {
              case "HighWritableTextBoxForValuesColumn1":
                HighWritableTextBoxForValuesColumn2.Focus();
                break;
              case "HighWritableTextBoxForValuesColumn2":
                HighWritableTextBoxForValuesColumn3.Focus();
                break;
              case "HighWritableTextBoxForValuesColumn3":
                HighWritableTextBoxForValuesColumn4.Focus();
                break;
            }
          }
        }
      }
    }
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

  public class HighWritableTextBoxColumn: System.Windows.Forms.TextBox {
    public HighWritableTextBoxColumn() {
      MinimumSize = new System.Drawing.Size(4 * ExcaliburComponents.SizeProportions.ScaleFactor, 7 * ExcaliburComponents.SizeProportions.ScaleFactor);
      MaximumSize = new System.Drawing.Size(4 * ExcaliburComponents.SizeProportions.ScaleFactor, 7 * ExcaliburComponents.SizeProportions.ScaleFactor);

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
      Size = new System.Drawing.Size(12 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
      
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
      Size = new System.Drawing.Size(16 * ExcaliburComponents.SizeProportions.ScaleFactor, 3 * ExcaliburComponents.SizeProportions.ScaleFactor);
    }
  }

  public class FlagButton: System.Windows.Forms.Button {
    public FlagButton() {
      Size = new System.Drawing.Size(3 * ExcaliburComponents.SizeProportions.ScaleFactor, 2 * ExcaliburComponents.SizeProportions.ScaleFactor);
      BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
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