namespace WinformTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConsume = new System.Windows.Forms.Button();
            this.btnProduce = new System.Windows.Forms.Button();
            this.txbConsume = new System.Windows.Forms.TextBox();
            this.txbProduce = new System.Windows.Forms.TextBox();
            this.btnExec = new System.Windows.Forms.Button();
            this.txbSQL = new System.Windows.Forms.TextBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnLink = new System.Windows.Forms.Button();
            this.btnStartService = new System.Windows.Forms.Button();
            this.btnStartWatcher = new System.Windows.Forms.Button();
            this.btnFormat = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRocketMQ = new System.Windows.Forms.Button();
            this.btnCommonjar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txbDll = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbJar = new System.Windows.Forms.TextBox();
            this.btnError = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.txbSelfPort = new System.Windows.Forms.TextBox();
            this.btnListen = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnPipeProxy = new System.Windows.Forms.Button();
            this.btnPipeWatcher = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnStartMonitor = new System.Windows.Forms.Button();
            this.btnStartWatchService = new System.Windows.Forms.Button();
            this.btnGetService = new System.Windows.Forms.Button();
            this.btnGetServiceInfo = new System.Windows.Forms.Button();
            this.btndb2 = new System.Windows.Forms.Button();
            this.btnIpCheck = new System.Windows.Forms.Button();
            this.btnAsync = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConsume
            // 
            this.btnConsume.Location = new System.Drawing.Point(29, 224);
            this.btnConsume.Name = "btnConsume";
            this.btnConsume.Size = new System.Drawing.Size(131, 23);
            this.btnConsume.TabIndex = 0;
            this.btnConsume.Text = "start listener";
            this.btnConsume.UseVisualStyleBackColor = true;
            this.btnConsume.Click += new System.EventHandler(this.btnConsume_Click);
            // 
            // btnProduce
            // 
            this.btnProduce.Location = new System.Drawing.Point(29, 280);
            this.btnProduce.Name = "btnProduce";
            this.btnProduce.Size = new System.Drawing.Size(131, 23);
            this.btnProduce.TabIndex = 1;
            this.btnProduce.Text = "produce message";
            this.btnProduce.UseVisualStyleBackColor = true;
            this.btnProduce.Click += new System.EventHandler(this.btnProduce_Click);
            // 
            // txbConsume
            // 
            this.txbConsume.Location = new System.Drawing.Point(29, 29);
            this.txbConsume.Multiline = true;
            this.txbConsume.Name = "txbConsume";
            this.txbConsume.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbConsume.Size = new System.Drawing.Size(712, 189);
            this.txbConsume.TabIndex = 2;
            // 
            // txbProduce
            // 
            this.txbProduce.Location = new System.Drawing.Point(29, 253);
            this.txbProduce.Name = "txbProduce";
            this.txbProduce.Size = new System.Drawing.Size(712, 21);
            this.txbProduce.TabIndex = 3;
            this.txbProduce.Text = "127.0.0.1";
            // 
            // btnExec
            // 
            this.btnExec.Location = new System.Drawing.Point(29, 770);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(75, 23);
            this.btnExec.TabIndex = 4;
            this.btnExec.Text = "sql exec";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // txbSQL
            // 
            this.txbSQL.Location = new System.Drawing.Point(29, 575);
            this.txbSQL.Multiline = true;
            this.txbSQL.Name = "txbSQL";
            this.txbSQL.Size = new System.Drawing.Size(712, 189);
            this.txbSQL.TabIndex = 5;
            this.txbSQL.Text = "SELECT BS,DATA,EVENTS,FORMATS,JLYBH,JLYXM,LAT,LON,SCBS,TIMES,TYPE,XYBH,XYXM,ZDID " +
    "FROM DB2INST1.G_MEDIA201707 fetch first 1000 rows only";
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(110, 770);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 6;
            this.btnInsert.Text = "insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(191, 770);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(75, 23);
            this.btnLink.TabIndex = 7;
            this.btnLink.Text = "link db2";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // btnStartService
            // 
            this.btnStartService.Location = new System.Drawing.Point(747, 27);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(131, 23);
            this.btnStartService.TabIndex = 8;
            this.btnStartService.Text = "start sync service";
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // btnStartWatcher
            // 
            this.btnStartWatcher.Location = new System.Drawing.Point(747, 56);
            this.btnStartWatcher.Name = "btnStartWatcher";
            this.btnStartWatcher.Size = new System.Drawing.Size(131, 23);
            this.btnStartWatcher.TabIndex = 9;
            this.btnStartWatcher.Text = "start table watcher";
            this.btnStartWatcher.UseVisualStyleBackColor = true;
            this.btnStartWatcher.Click += new System.EventHandler(this.btnStartWatcher_Click);
            // 
            // btnFormat
            // 
            this.btnFormat.Location = new System.Drawing.Point(29, 309);
            this.btnFormat.Name = "btnFormat";
            this.btnFormat.Size = new System.Drawing.Size(131, 23);
            this.btnFormat.TabIndex = 10;
            this.btnFormat.Text = "format";
            this.btnFormat.UseVisualStyleBackColor = true;
            this.btnFormat.Click += new System.EventHandler(this.btnFormat_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRocketMQ);
            this.groupBox1.Controls.Add(this.btnCommonjar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txbDll);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txbJar);
            this.groupBox1.Location = new System.Drawing.Point(747, 575);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(749, 189);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "jartodll";
            // 
            // btnRocketMQ
            // 
            this.btnRocketMQ.Location = new System.Drawing.Point(116, 83);
            this.btnRocketMQ.Name = "btnRocketMQ";
            this.btnRocketMQ.Size = new System.Drawing.Size(102, 23);
            this.btnRocketMQ.TabIndex = 5;
            this.btnRocketMQ.Text = "转换rocketmq";
            this.btnRocketMQ.UseVisualStyleBackColor = true;
            this.btnRocketMQ.Click += new System.EventHandler(this.btnRocketMQ_Click);
            // 
            // btnCommonjar
            // 
            this.btnCommonjar.Location = new System.Drawing.Point(8, 83);
            this.btnCommonjar.Name = "btnCommonjar";
            this.btnCommonjar.Size = new System.Drawing.Size(102, 23);
            this.btnCommonjar.TabIndex = 4;
            this.btnCommonjar.Text = "转换普通jar";
            this.btnCommonjar.UseVisualStyleBackColor = true;
            this.btnCommonjar.Click += new System.EventHandler(this.btnCommonjar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "dll路径";
            // 
            // txbDll
            // 
            this.txbDll.Location = new System.Drawing.Point(72, 48);
            this.txbDll.Name = "txbDll";
            this.txbDll.Size = new System.Drawing.Size(671, 21);
            this.txbDll.TabIndex = 2;
            this.txbDll.Text = "E:\\jartodll\\4.2dll";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "jar路径";
            // 
            // txbJar
            // 
            this.txbJar.Location = new System.Drawing.Point(72, 21);
            this.txbJar.Name = "txbJar";
            this.txbJar.Size = new System.Drawing.Size(671, 21);
            this.txbJar.TabIndex = 0;
            this.txbJar.Text = "E:\\jartodll\\4.2jar";
            // 
            // btnError
            // 
            this.btnError.Location = new System.Drawing.Point(747, 85);
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(131, 23);
            this.btnError.TabIndex = 12;
            this.btnError.Text = "start error watcher";
            this.btnError.UseVisualStyleBackColor = true;
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(29, 546);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "socket";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(166, 548);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(55, 21);
            this.txbPort.TabIndex = 14;
            this.txbPort.Text = "9876";
            // 
            // txbSelfPort
            // 
            this.txbSelfPort.Location = new System.Drawing.Point(549, 548);
            this.txbSelfPort.Name = "txbSelfPort";
            this.txbSelfPort.Size = new System.Drawing.Size(55, 21);
            this.txbSelfPort.TabIndex = 15;
            this.txbSelfPort.Text = "9876";
            // 
            // btnListen
            // 
            this.btnListen.Location = new System.Drawing.Point(610, 546);
            this.btnListen.Name = "btnListen";
            this.btnListen.Size = new System.Drawing.Size(131, 23);
            this.btnListen.TabIndex = 16;
            this.btnListen.Text = "listen";
            this.btnListen.UseVisualStyleBackColor = true;
            this.btnListen.Click += new System.EventHandler(this.btnListen_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(610, 488);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(131, 23);
            this.btnSend.TabIndex = 17;
            this.btnSend.Text = "send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnPipeProxy
            // 
            this.btnPipeProxy.Location = new System.Drawing.Point(747, 166);
            this.btnPipeProxy.Name = "btnPipeProxy";
            this.btnPipeProxy.Size = new System.Drawing.Size(131, 23);
            this.btnPipeProxy.TabIndex = 18;
            this.btnPipeProxy.Text = "start pipe proxy";
            this.btnPipeProxy.UseVisualStyleBackColor = true;
            this.btnPipeProxy.Click += new System.EventHandler(this.btnPipeProxy_Click);
            // 
            // btnPipeWatcher
            // 
            this.btnPipeWatcher.Location = new System.Drawing.Point(747, 195);
            this.btnPipeWatcher.Name = "btnPipeWatcher";
            this.btnPipeWatcher.Size = new System.Drawing.Size(131, 23);
            this.btnPipeWatcher.TabIndex = 19;
            this.btnPipeWatcher.Text = "start pipe watcher";
            this.btnPipeWatcher.UseVisualStyleBackColor = true;
            this.btnPipeWatcher.Click += new System.EventHandler(this.btnPipeWatcher_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(610, 517);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(131, 23);
            this.btnConnect.TabIndex = 20;
            this.btnConnect.Text = "connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(755, 253);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(131, 23);
            this.btnProcess.TabIndex = 21;
            this.btnProcess.Text = "process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // btnStartMonitor
            // 
            this.btnStartMonitor.Location = new System.Drawing.Point(755, 282);
            this.btnStartMonitor.Name = "btnStartMonitor";
            this.btnStartMonitor.Size = new System.Drawing.Size(131, 23);
            this.btnStartMonitor.TabIndex = 22;
            this.btnStartMonitor.Text = "start monitor";
            this.btnStartMonitor.UseVisualStyleBackColor = true;
            this.btnStartMonitor.Click += new System.EventHandler(this.btnStartMonitor_Click);
            // 
            // btnStartWatchService
            // 
            this.btnStartWatchService.Location = new System.Drawing.Point(755, 311);
            this.btnStartWatchService.Name = "btnStartWatchService";
            this.btnStartWatchService.Size = new System.Drawing.Size(131, 23);
            this.btnStartWatchService.TabIndex = 23;
            this.btnStartWatchService.Text = "start service";
            this.btnStartWatchService.UseVisualStyleBackColor = true;
            this.btnStartWatchService.Click += new System.EventHandler(this.btnStartWatchService_Click);
            // 
            // btnGetService
            // 
            this.btnGetService.Location = new System.Drawing.Point(29, 338);
            this.btnGetService.Name = "btnGetService";
            this.btnGetService.Size = new System.Drawing.Size(156, 23);
            this.btnGetService.TabIndex = 24;
            this.btnGetService.Text = "get windows service";
            this.btnGetService.UseVisualStyleBackColor = true;
            this.btnGetService.Click += new System.EventHandler(this.btnGetService_Click);
            // 
            // btnGetServiceInfo
            // 
            this.btnGetServiceInfo.Location = new System.Drawing.Point(29, 367);
            this.btnGetServiceInfo.Name = "btnGetServiceInfo";
            this.btnGetServiceInfo.Size = new System.Drawing.Size(156, 23);
            this.btnGetServiceInfo.TabIndex = 25;
            this.btnGetServiceInfo.Text = "get service info";
            this.btnGetServiceInfo.UseVisualStyleBackColor = true;
            this.btnGetServiceInfo.Click += new System.EventHandler(this.btnGetServiceInfo_Click);
            // 
            // btndb2
            // 
            this.btndb2.Location = new System.Drawing.Point(227, 546);
            this.btndb2.Name = "btndb2";
            this.btndb2.Size = new System.Drawing.Size(131, 23);
            this.btndb2.TabIndex = 26;
            this.btndb2.Text = "db2";
            this.btndb2.UseVisualStyleBackColor = true;
            this.btndb2.Click += new System.EventHandler(this.btndb2_Click);
            // 
            // btnIpCheck
            // 
            this.btnIpCheck.Location = new System.Drawing.Point(610, 459);
            this.btnIpCheck.Name = "btnIpCheck";
            this.btnIpCheck.Size = new System.Drawing.Size(131, 23);
            this.btnIpCheck.TabIndex = 27;
            this.btnIpCheck.Text = "check";
            this.btnIpCheck.UseVisualStyleBackColor = true;
            this.btnIpCheck.Click += new System.EventHandler(this.btnIpCheck_Click);
            // 
            // btnAsync
            // 
            this.btnAsync.Location = new System.Drawing.Point(747, 546);
            this.btnAsync.Name = "btnAsync";
            this.btnAsync.Size = new System.Drawing.Size(131, 23);
            this.btnAsync.TabIndex = 28;
            this.btnAsync.Text = "async";
            this.btnAsync.UseVisualStyleBackColor = true;
            this.btnAsync.Click += new System.EventHandler(this.btnAsync_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1508, 805);
            this.Controls.Add(this.btnAsync);
            this.Controls.Add(this.btnIpCheck);
            this.Controls.Add(this.btndb2);
            this.Controls.Add(this.btnGetServiceInfo);
            this.Controls.Add(this.btnGetService);
            this.Controls.Add(this.btnStartWatchService);
            this.Controls.Add(this.btnStartMonitor);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnPipeWatcher);
            this.Controls.Add(this.btnPipeProxy);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnListen);
            this.Controls.Add(this.txbSelfPort);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnError);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnFormat);
            this.Controls.Add(this.btnStartWatcher);
            this.Controls.Add(this.btnStartService);
            this.Controls.Add(this.btnLink);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.txbSQL);
            this.Controls.Add(this.btnExec);
            this.Controls.Add(this.txbProduce);
            this.Controls.Add(this.txbConsume);
            this.Controls.Add(this.btnProduce);
            this.Controls.Add(this.btnConsume);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConsume;
        private System.Windows.Forms.Button btnProduce;
        private System.Windows.Forms.TextBox txbConsume;
        private System.Windows.Forms.TextBox txbProduce;
        private System.Windows.Forms.Button btnExec;
        private System.Windows.Forms.TextBox txbSQL;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.Button btnStartWatcher;
        private System.Windows.Forms.Button btnFormat;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRocketMQ;
        private System.Windows.Forms.Button btnCommonjar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbDll;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbJar;
        private System.Windows.Forms.Button btnError;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.TextBox txbSelfPort;
        private System.Windows.Forms.Button btnListen;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnPipeProxy;
        private System.Windows.Forms.Button btnPipeWatcher;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Button btnStartMonitor;
        private System.Windows.Forms.Button btnStartWatchService;
        private System.Windows.Forms.Button btnGetService;
        private System.Windows.Forms.Button btnGetServiceInfo;
        private System.Windows.Forms.Button btndb2;
        private System.Windows.Forms.Button btnIpCheck;
        private System.Windows.Forms.Button btnAsync;
    }
}

