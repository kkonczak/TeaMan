﻿using System.Reactive;
using System.Windows;
using TeaMan.Interactions;

namespace TeaMan.Views
{
    /// <summary>
    /// Interaction logic for AddTaskTypeView.xaml
    /// </summary>
    public partial class AddTaskTypeView : Window
    {
        public AddTaskTypeView()
        {
            InitializeComponent();

            CloseWindowInteraction.CloseWindow.RegisterHandler(
                interaction =>
                {
                    this.DialogResult = interaction.Input;
                    this.Close();

                    interaction.SetOutput(Unit.Default);
                });
        }
    }
}
