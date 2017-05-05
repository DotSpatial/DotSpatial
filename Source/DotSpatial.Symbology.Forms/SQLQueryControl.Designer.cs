﻿using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    partial class SqlQueryControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlQueryControl));
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFields = new System.Windows.Forms.Label();
            this.lblUniqueValues = new System.Windows.Forms.Label();
            this.lblSelectPrecursor = new System.Windows.Forms.Label();
            this.rtbFilterText = new System.Windows.Forms.RichTextBox();
            this.btnGetUniqueValues = new System.Windows.Forms.Button();
            this.btnNot = new System.Windows.Forms.Button();
            this.btnParenthasis = new System.Windows.Forms.Button();
            this.btnAsterix = new System.Windows.Forms.Button();
            this.btnOr = new System.Windows.Forms.Button();
            this.btnLessThanOrEqual = new System.Windows.Forms.Button();
            this.btnLessThan = new System.Windows.Forms.Button();
            this.btnAnd = new System.Windows.Forms.Button();
            this.btnGreaterThanOrEqual = new System.Windows.Forms.Button();
            this.btnGreaterThan = new System.Windows.Forms.Button();
            this.btnLike = new System.Windows.Forms.Button();
            this.btnNotEqual = new System.Windows.Forms.Button();
            this.btnEquals = new System.Windows.Forms.Button();
            this.lbxUniqueValues = new System.Windows.Forms.ListBox();
            this.lbxFields = new System.Windows.Forms.ListBox();
            this.btnNull = new System.Windows.Forms.Button();
            this.btnNotNull = new System.Windows.Forms.Button();
            this.btnValidate = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMax
            // 
            resources.ApplyResources(this.lblMax, "lblMax");
            this.lblMax.AutoEllipsis = true;
            this.lblMax.BackColor = System.Drawing.Color.White;
            this.lblMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMax.Name = "lblMax";
            // 
            // lblMin
            // 
            resources.ApplyResources(this.lblMin, "lblMin");
            this.lblMin.AutoEllipsis = true;
            this.lblMin.BackColor = System.Drawing.Color.White;
            this.lblMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMin.Name = "lblMin";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblFields
            // 
            resources.ApplyResources(this.lblFields, "lblFields");
            this.lblFields.Name = "lblFields";
            // 
            // lblUniqueValues
            // 
            resources.ApplyResources(this.lblUniqueValues, "lblUniqueValues");
            this.lblUniqueValues.Name = "lblUniqueValues";
            // 
            // lblSelectPrecursor
            // 
            resources.ApplyResources(this.lblSelectPrecursor, "lblSelectPrecursor");
            this.lblSelectPrecursor.Name = "lblSelectPrecursor";
            // 
            // rtbFilterText
            // 
            resources.ApplyResources(this.rtbFilterText, "rtbFilterText");
            this.rtbFilterText.Name = "rtbFilterText";
            this.rtbFilterText.TextChanged += new System.EventHandler(this.RtbFilterTextTextChanged);
            // 
            // btnGetUniqueValues
            // 
            resources.ApplyResources(this.btnGetUniqueValues, "btnGetUniqueValues");
            this.btnGetUniqueValues.Name = "btnGetUniqueValues";
            this.btnGetUniqueValues.UseVisualStyleBackColor = true;
            this.btnGetUniqueValues.Click += new System.EventHandler(this.BtnGetUniqueValuesClick);
            // 
            // btnNot
            // 
            resources.ApplyResources(this.btnNot, "btnNot");
            this.btnNot.Name = "btnNot";
            this.btnNot.UseVisualStyleBackColor = true;
            this.btnNot.Click += new System.EventHandler(this.BtnNotClick);
            // 
            // btnParenthasis
            // 
            resources.ApplyResources(this.btnParenthasis, "btnParenthasis");
            this.btnParenthasis.Name = "btnParenthasis";
            this.btnParenthasis.UseVisualStyleBackColor = true;
            this.btnParenthasis.Click += new System.EventHandler(this.BtnParenthasisClick);
            // 
            // btnAsterix
            // 
            resources.ApplyResources(this.btnAsterix, "btnAsterix");
            this.btnAsterix.Name = "btnAsterix";
            this.btnAsterix.UseVisualStyleBackColor = true;
            this.btnAsterix.Click += new System.EventHandler(this.BtnAsterixClick);
            // 
            // btnOr
            // 
            resources.ApplyResources(this.btnOr, "btnOr");
            this.btnOr.Name = "btnOr";
            this.btnOr.UseVisualStyleBackColor = true;
            this.btnOr.Click += new System.EventHandler(this.BtnOrClick);
            // 
            // btnLessThanOrEqual
            // 
            resources.ApplyResources(this.btnLessThanOrEqual, "btnLessThanOrEqual");
            this.btnLessThanOrEqual.Name = "btnLessThanOrEqual";
            this.btnLessThanOrEqual.UseVisualStyleBackColor = true;
            this.btnLessThanOrEqual.Click += new System.EventHandler(this.BtnLessThanOrEqualClick);
            // 
            // btnLessThan
            // 
            resources.ApplyResources(this.btnLessThan, "btnLessThan");
            this.btnLessThan.Name = "btnLessThan";
            this.btnLessThan.UseVisualStyleBackColor = true;
            this.btnLessThan.Click += new System.EventHandler(this.BtnLessThanClick);
            // 
            // btnAnd
            // 
            resources.ApplyResources(this.btnAnd, "btnAnd");
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.UseVisualStyleBackColor = true;
            this.btnAnd.Click += new System.EventHandler(this.BtnAndClick);
            // 
            // btnGreaterThanOrEqual
            // 
            resources.ApplyResources(this.btnGreaterThanOrEqual, "btnGreaterThanOrEqual");
            this.btnGreaterThanOrEqual.Name = "btnGreaterThanOrEqual";
            this.btnGreaterThanOrEqual.UseVisualStyleBackColor = true;
            this.btnGreaterThanOrEqual.Click += new System.EventHandler(this.BtnGreaterThanOrEqualClick);
            // 
            // btnGreaterThan
            // 
            resources.ApplyResources(this.btnGreaterThan, "btnGreaterThan");
            this.btnGreaterThan.Name = "btnGreaterThan";
            this.btnGreaterThan.UseVisualStyleBackColor = true;
            this.btnGreaterThan.Click += new System.EventHandler(this.BtnGreaterThanClick);
            // 
            // btnLike
            // 
            resources.ApplyResources(this.btnLike, "btnLike");
            this.btnLike.Name = "btnLike";
            this.btnLike.UseVisualStyleBackColor = true;
            this.btnLike.Click += new System.EventHandler(this.BtnLikeClick);
            // 
            // btnNotEqual
            // 
            resources.ApplyResources(this.btnNotEqual, "btnNotEqual");
            this.btnNotEqual.Name = "btnNotEqual";
            this.btnNotEqual.UseVisualStyleBackColor = true;
            this.btnNotEqual.Click += new System.EventHandler(this.BtnNotEqualClick);
            // 
            // btnEquals
            // 
            resources.ApplyResources(this.btnEquals, "btnEquals");
            this.btnEquals.Name = "btnEquals";
            this.btnEquals.UseVisualStyleBackColor = true;
            this.btnEquals.Click += new System.EventHandler(this.BtnEqualsClick);
            // 
            // lbxUniqueValues
            // 
            resources.ApplyResources(this.lbxUniqueValues, "lbxUniqueValues");
            this.lbxUniqueValues.BackColor = System.Drawing.SystemColors.Control;
            this.lbxUniqueValues.FormattingEnabled = true;
            this.lbxUniqueValues.Name = "lbxUniqueValues";
            this.lbxUniqueValues.DoubleClick += new System.EventHandler(this.LbxUniqueValuesDoubleClick);
            // 
            // lbxFields
            // 
            resources.ApplyResources(this.lbxFields, "lbxFields");
            this.lbxFields.FormattingEnabled = true;
            this.lbxFields.Name = "lbxFields";
            this.lbxFields.SelectedIndexChanged += new System.EventHandler(this.LbxFieldsSelectedIndexChanged);
            this.lbxFields.DoubleClick += new System.EventHandler(this.LbxFieldsDoubleClick);
            // 
            // btnNull
            // 
            resources.ApplyResources(this.btnNull, "btnNull");
            this.btnNull.Name = "btnNull";
            this.btnNull.UseVisualStyleBackColor = true;
            this.btnNull.Click += new System.EventHandler(this.BtnNullClick);
            // 
            // btnNotNull
            // 
            resources.ApplyResources(this.btnNotNull, "btnNotNull");
            this.btnNotNull.Name = "btnNotNull";
            this.btnNotNull.UseVisualStyleBackColor = true;
            this.btnNotNull.Click += new System.EventHandler(this.BtnNotNullClick);
            // 
            // btnValidate
            // 
            resources.ApplyResources(this.btnValidate, "btnValidate");
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.BtnValidateClick);
            // 
            // lblResult
            // 
            resources.ApplyResources(this.lblResult, "lblResult");
            this.lblResult.Name = "lblResult";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // SQLQueryControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnValidate);
            this.Controls.Add(this.btnNotNull);
            this.Controls.Add(this.btnNull);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFields);
            this.Controls.Add(this.lblUniqueValues);
            this.Controls.Add(this.lblSelectPrecursor);
            this.Controls.Add(this.rtbFilterText);
            this.Controls.Add(this.btnGetUniqueValues);
            this.Controls.Add(this.btnNot);
            this.Controls.Add(this.btnParenthasis);
            this.Controls.Add(this.btnAsterix);
            this.Controls.Add(this.btnOr);
            this.Controls.Add(this.btnLessThanOrEqual);
            this.Controls.Add(this.btnLessThan);
            this.Controls.Add(this.btnAnd);
            this.Controls.Add(this.btnGreaterThanOrEqual);
            this.Controls.Add(this.btnGreaterThan);
            this.Controls.Add(this.btnLike);
            this.Controls.Add(this.btnNotEqual);
            this.Controls.Add(this.btnEquals);
            this.Controls.Add(this.lbxUniqueValues);
            this.Controls.Add(this.lbxFields);
            this.Name = "SqlQueryControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private Button btnAnd;
        private Button btnAsterix;
        private Button btnEquals;
        private Button btnGetUniqueValues;
        private Button btnGreaterThan;
        private Button btnGreaterThanOrEqual;
        private Button btnLessThan;
        private Button btnLessThanOrEqual;
        private Button btnLike;
        private Button btnNot;
        private Button btnNotEqual;
        private Button btnNotNull;
        private Button btnNull;
        private Button btnOr;
        private Button btnParenthasis;
        private Label label1;
        private Label label2;
        private Label lblFields;
        private Label lblMax;
        private Label lblMin;
        private Label lblSelectPrecursor;
        private Label lblUniqueValues;
        private ListBox lbxFields;
        private ListBox lbxUniqueValues;
        private RichTextBox rtbFilterText;
        private Button btnValidate;
        private Label lblResult;
        private Label label3;
    }
}
