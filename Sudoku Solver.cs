﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sudoku_Solver
{
    public partial class SudokuSolver : Form
    {
        public SudokuSolver()
        {
            InitializeComponent();

            //set button widths
            int SideControlWidth = 250;
            SetNumbers.Width = SingleSolve.Width = ShowPossibleValues.Width = StatusBox.Width = SolveMultiRun.Width = StartGuess.Width = SideControlWidth;

            //set form width and height based on controls
            this.Width = FORM_WIDTH + SetNumbers.Width + 25;
            this.Height = FORM_HEIGHT + 60;

            //set button left
            int SideControlMargin = 13;
            SetNumbers.Left = this.Width - SetNumbers.Width - SideControlMargin;
            SingleSolve.Left = this.Width - SingleSolve.Width - SideControlMargin;
            SolveMultiRun.Left = this.Width - SolveMultiRun.Width - SideControlMargin;
            ShowPossibleValues.Left = this.Width - ShowPossibleValues.Width - SideControlMargin;
            StartGuess.Left = this.Width - StartGuess.Width - SideControlMargin;
            StatusBox.Left = this.Width - StatusBox.Width - SideControlMargin;
            StatusLabel.Text = "0 Guess Made";
        }

        private void SudokuSolver_Load(object sender, EventArgs e)
        {
            SetupBlocks();
            SetTestNumber();
        }

        private void SetNumbers_Click(object sender, EventArgs e)
        {
            if (!NumberSet)
            {
                SetOriginalNumber(true);
                NumberSet = true;
                SetNumbers.Text = "Unset Numbers";
                AppendStatus("Numbers Locked");
            }
            else
            {
                SetOriginalNumber(false);
                NumberSet = false;
                SetNumbers.Text = "Set Numbers";
                AppendStatus("Numbers Unocked");
            }

        }

        private void SingleSolve_Click(object sender, EventArgs e)
        {
            if (NumberSet)
            {
                //ClearCellStyle(CellStyleState.Checked);
                SolveSingleRun();
            }
            else
            {
                AppendStatus("Set Numbers First, Dumbass.");
            }
        }

        private void SolveMultiRun_Click(object sender, EventArgs e)
        {
            if (NumberSet)
            {
                SolveMultiRuns();
            }
            else
            {
                AppendStatus("Set Numbers First, Dumbass.");
            }
        }

        private void ShowPossibleValues_Click(object sender, EventArgs e)
        {
            if (NumberSet)
            {
                if (!SolveMultiRuns())
                {
                    DisplayAllPossibleValues();

                    List<Guess> BestGuesses = GetBestGuesses();
                    UpdateStatusProgressMaximum(BestGuesses.Count);
                    
                    UpdateStatusProgress(BestGuesses.Count(), 0, GetPossibleGuess());
                }
            }
            else
            {
                AppendStatus("Set Numbers First, Dumbass.");
            }
        }

        private void StartGuess_Click(object sender, EventArgs e)
        {
            if (NumberSet)
            {
                DisplayAllPossibleValues();
                if (!SolveMultiRuns())
                {
                    BackgroundWorker GuessWorker = new BackgroundWorker();
                    GuessWorker.DoWork += BackgroundWork;
                    GuessWorker.RunWorkerAsync();
                    
                }
            }
            else
            {
                AppendStatus("Set Numbers First, Dumbass.");
            }
        }

        private void BackgroundWork(object sender, EventArgs e)
        {
            DateTime GuessStart = DateTime.Now;
            StartGuessing();
            TimeSpan GuessDuration = DateTime.Now.Subtract(GuessStart);

            if (IsGameSolved())
            {
                AppendStatus("Puzzled solved. It took " + Math.Round(GuessDuration.TotalSeconds, 2).ToString() + " seconds.");
            }
        }
    }
}