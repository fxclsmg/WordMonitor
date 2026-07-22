using WordMonitor.Configurator.Services;
using System.Text.Json.Nodes;

namespace WordMonitor.Configurator;

public class MainForm : Form
{
    private TextBox txtPasta;
    private Button btnProcurar;

    private NumericUpDown nudDiasAviso;
    private DateTimePicker dtHorario;

    private CheckBox chkLog;
    private CheckBox chkEmail;

    private TextBox txtServidor;
    private NumericUpDown nudPorta;
    private TextBox txtUsuario;
    private TextBox txtSenha;
    private TextBox txtDestinatario;
    private Button btnMostrarSenha;

    private Button btnSalvar;
    private Button btnCancelar;

    private ConfigService _configService;
    private readonly ServiceManager _serviceManager;
    private JsonObject _config; 

    public MainForm()
    {

        var caminhoInstalado = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            "WordMonitor",
            "appsettings.Local.json"
        );

        var caminhoLocal = Path.Combine(
            AppContext.BaseDirectory,
            "appsettings.Local.json");

        _configService = new ConfigService(
            File.Exists(caminhoInstalado)
                ? caminhoInstalado
                : caminhoLocal
        );

        _config = _configService.Carregar();

        var caminhoServico = Path.Combine(
            AppContext.BaseDirectory,
            "WordMonitor.Worker.exe");

        _serviceManager = new ServiceManager();

        InicializarTela();

        CarregarConfiguracoes();
    }


    private void InicializarTela()
    {
        Text = "WordMonitor - Configurador";
        Width = 20 + 600 + 20;
        Height = 20 + 620 + 20 ;

        StartPosition = FormStartPosition.CenterScreen;


        // =========================
        // Monitoramento
        // =========================

        var grupoMonitor = new GroupBox();

        grupoMonitor.Text = "Monitoramento";
        grupoMonitor.Left = 20;
        grupoMonitor.Top = 20;
        grupoMonitor.Width = 580;
        grupoMonitor.Height = 150;


        var lblPasta = new Label();

        lblPasta.Text = "Pasta monitorada:";
        lblPasta.Left = 20;
        lblPasta.Top = 30;
        lblPasta.Width = 120;


        txtPasta = new TextBox();

        txtPasta.Left = 150;
        txtPasta.Top = 25;
        txtPasta.Width = 300;


        btnProcurar = new Button();

        btnProcurar.Text = "Procurar";
        btnProcurar.Left = 460;
        btnProcurar.Top = 25;
        btnProcurar.Width = 80;

        btnProcurar.Click += BtnProcurar_Click;


        var lblDias = new Label();

        lblDias.Text = "Dias de aviso:";
        lblDias.Left = 20;
        lblDias.Top = 70;


        nudDiasAviso = new NumericUpDown();

        nudDiasAviso.Left = 150;
        nudDiasAviso.Top = 65;
        nudDiasAviso.Minimum = 1;
        nudDiasAviso.Maximum = 365;
        nudDiasAviso.Value = 3;
        nudDiasAviso.Width = 50;


        var lblHorario = new Label();

        lblHorario.Text = "Horário:";
        lblHorario.Left = 20;
        lblHorario.Top = 110;


        dtHorario = new DateTimePicker();

        dtHorario.Left = 150;
        dtHorario.Top = 105;
        dtHorario.Format = DateTimePickerFormat.Time;
        dtHorario.ShowUpDown = true;


        grupoMonitor.Controls.Add(lblPasta);
        grupoMonitor.Controls.Add(txtPasta);
        grupoMonitor.Controls.Add(btnProcurar);
        grupoMonitor.Controls.Add(lblDias);
        grupoMonitor.Controls.Add(nudDiasAviso);
        grupoMonitor.Controls.Add(lblHorario);
        grupoMonitor.Controls.Add(dtHorario);



        // =========================
        // Email
        // =========================

        var grupoEmail = new GroupBox();

        grupoEmail.Text = "E-mail";
        grupoEmail.Left = 20;
        grupoEmail.Top = grupoMonitor.Top + grupoMonitor.Height + 20;
        grupoEmail.Width = 580;
        grupoEmail.Height = 270;


        txtServidor = CriarCampo(
            grupoEmail,
            "Servidor SMTP:",
            25
        );


        var lblPorta = new Label();

        lblPorta.Text = "Porta:";
        lblPorta.Left = 20;
        lblPorta.Top = txtServidor.Top + txtServidor.Height + 30;

        grupoEmail.Controls.Add(lblPorta);
  

        nudPorta = new NumericUpDown();

        nudPorta.Left = 150;
        nudPorta.Top = txtServidor.Top + txtServidor.Height + 25;;
        nudPorta.Minimum = 1;
        nudPorta.Maximum = 65535;
        nudPorta.Value = 587;
        nudPorta.Width = 50;

        grupoEmail.Controls.Add(nudPorta);


        txtUsuario = CriarCampo(
            grupoEmail,
            "Usuário:",
            nudPorta.Top + nudPorta.Height + 25
        );


        txtSenha = CriarCampo(
            grupoEmail,
            "Senha:",
            txtUsuario.Top + txtUsuario.Height + 25
        );

        txtSenha.PasswordChar = '*';


        btnMostrarSenha = new Button();

        btnMostrarSenha.Text = "👁";

        btnMostrarSenha.Left = 460;
        btnMostrarSenha.Top = txtUsuario.Top + txtUsuario.Height + 25;
        btnMostrarSenha.Width = 50;

        btnMostrarSenha.Click += BtnMostrarSenha_Click;


        grupoEmail.Controls.Add(btnMostrarSenha);

        txtDestinatario = CriarCampo(
            grupoEmail,
            "Destinatário:",
            txtSenha.Top + txtSenha.Height + 25
        );


        // =========================
        // Notificação
        // =========================

        var grupoNotificacao = new GroupBox();

        grupoNotificacao.Text = "Notificação";
        grupoNotificacao.Left = 20;
        grupoNotificacao.Top = grupoEmail.Top + grupoEmail.Height + 20;
        grupoNotificacao.Width = 580;
        grupoNotificacao.Height = 70;
        
        chkLog = new CheckBox();
        chkLog.Text = "Ativar Log";
        chkLog.Left = 20;
        chkLog.Top = 30;


        chkEmail = new CheckBox();
        chkEmail.Text = "Ativar Email";
        chkEmail.Left = 150;
        chkEmail.Top = 30;


        grupoNotificacao.Controls.Add(chkLog);
        grupoNotificacao.Controls.Add(chkEmail);


        // =========================
        // Botões
        // =========================

        btnSalvar = new Button();

        btnSalvar.Text = "Salvar";
        btnSalvar.Left = 380;
        btnSalvar.Top = grupoNotificacao.Top + grupoNotificacao.Height + 20;
        btnSalvar.Width = 100;

        btnSalvar.Click += BtnSalvar_Click;



        btnCancelar = new Button();

        btnCancelar.Text = "Cancelar";
        btnCancelar.Left = 490;
        btnCancelar.Top = grupoNotificacao.Top + grupoNotificacao.Height + 20;
        btnCancelar.Width = 100;

        btnCancelar.Click += BtnCancelar_Click;

        Controls.Add(grupoMonitor);
        Controls.Add(grupoEmail);
        Controls.Add(grupoNotificacao);
        Controls.Add(btnSalvar);
        Controls.Add(btnCancelar);
    }


    private TextBox CriarCampo(
        GroupBox grupo,
        string texto,
        int top)
    {
        var label = new Label();

        label.Text = texto;
        label.Left = 20;
        label.Top = top + 5;
        label.Width = 120;


        var caixa = new TextBox();

        caixa.Left = 150;
        caixa.Top = top;
        caixa.Width = 300;


        grupo.Controls.Add(label);
        grupo.Controls.Add(caixa);


        return caixa;
    }

    private void CarregarConfiguracoes()
    {
        txtPasta.Text =
            _config["Monitor"]?["Pasta"]?.ToString();

        nudDiasAviso.Value =
            int.Parse(
                _config["Validade"]?["DiasAviso"]?.ToString() ?? "3"
            );

        dtHorario.Value =
            DateTime.Parse(
                _config["Verificacao"]?["Horario"]?.ToString()
                ?? "15:00"
            );

        txtServidor.Text =
            _config["Email"]?["Servidor"]?.ToString();

        txtUsuario.Text =
            _config["Email"]?["Usuario"]?.ToString();

        txtSenha.Text =
            _config["Email"]?["Senha"]?.ToString();

        txtDestinatario.Text =
            _config["Email"]?["Destinatario"]?.ToString();

        chkLog.Checked = 
            _config["Notificar"]?["Log"]?.GetValue<bool>() ?? false;
            
        chkEmail.Checked = 
            _config["Notificar"]?["Email"]?.GetValue<bool>() ?? false;
    }

    private void BtnSalvar_Click(object? sender, EventArgs e)
    {
        SalvarConfiguracao();

        InstalarOuAtualizarServico();

        Close();
    }

    private void InstalarOuAtualizarServico()
    {
        if (!_serviceManager.Existe())
                _serviceManager.Instalar();

            _serviceManager.Iniciar();
    }

    private void SalvarConfiguracao()
    {
        try
        {
            // Monitor
            if (_config["Monitor"] is JsonObject monitor)
            {
                monitor["Pasta"] = txtPasta.Text;
            }


            // Validade
            if (_config["Validade"] is JsonObject validade)
            {
                validade["DiasAviso"] = (int)nudDiasAviso.Value;
            }


            // Verificação
            if (_config["Verificacao"] is JsonObject verificacao)
            {
                verificacao["Horario"] =
                    dtHorario.Value.ToString("HH:mm");
            }


            // Email
            if (_config["Email"] is JsonObject email)
            {
                email["Servidor"] = txtServidor.Text;

                email["Porta"] =
                    (int)nudPorta.Value;

                email["Usuario"] =
                    txtUsuario.Text;

                email["Senha"] =
                    txtSenha.Text;

                email["Destinatario"] =
                    txtDestinatario.Text;
            }

            // Notificar
            if (_config["Notificar"] is JsonObject notificar)
            {
                notificar["Log"] = chkLog.Checked;
                notificar["Email"] = chkEmail.Checked;
            }

            _configService.Salvar(_config);

            MessageBox.Show(
                "Configurações salvas com sucesso.",
                "WordMonitor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

        }
        catch(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Erro ao salvar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void BtnMostrarSenha_Click(object? sender, EventArgs e)
    {
        if (txtSenha.PasswordChar == '*')
        {
            txtSenha.PasswordChar = '\0';
            btnMostrarSenha.Text = "🔒";
        }
        else
        {
            txtSenha.PasswordChar = '*';
            btnMostrarSenha.Text = "👁";
        }
    }

    private void BtnProcurar_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();

        dialog.Description = "Selecione a pasta que será monitorada.";

        dialog.ShowNewFolderButton = true;

        if (!string.IsNullOrWhiteSpace(txtPasta.Text) &&
            Directory.Exists(txtPasta.Text))
        {
            dialog.InitialDirectory = txtPasta.Text;
        }

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtPasta.Text = dialog.SelectedPath;
        }
    }

        private void BtnCancelar_Click(object? sender,EventArgs e)
    {
        Close();
    }
}
