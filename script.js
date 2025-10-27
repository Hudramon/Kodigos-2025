const { createApp } = Vue
const API_BASE_URL = 'http://localhost:5007';

createApp({
  data() {
    return {
      logado: false, 
      login: { usuario: '', senha: '', erro: false },
      os: { descricao: '', checklist: [], fotoPreview: null },
      mensagemSucesso: ''
    }
  },
  created() {
    if (sessionStorage.getItem('usuarioLogado') === 'true') {
      this.logado = true;
    }
  },
  methods: {
    fazerLogin() {
      // ATENÇÃO: A senha  é 'admin123'
      if (this.login.usuario === 'admin' && this.login.senha === 'admin123') {
        sessionStorage.setItem('usuarioLogado', 'true'); 
        this.logado = true;
        this.login.erro = false;
      } else {
        this.login.erro = true;
        sessionStorage.removeItem('usuarioLogado');
      }
    },
    // Função para sair
    fazerLogout() {
      sessionStorage.removeItem('usuarioLogado');
      window.location.reload();
    },
    carregarFoto(event) {
      const file = event.target.files[0];
      if (file) { this.os.fotoPreview = URL.createObjectURL(file); }
    },
    async salvarOS() {
      const dadosParaApi = {
        descricao: this.os.descricao,
        checklist: this.os.checklist.join(', ')
      };
      
      try {
        const response = await fetch(`${API_BASE_URL}/ordensdeservico`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(dadosParaApi),
        });

        if (!response.ok) { throw new Error('Falha ao salvar'); }

        this.mensagemSucesso = 'Ordem de Serviço salva com sucesso!';
        setTimeout(() => { this.mensagemSucesso = ''; }, 3000);

        this.os.descricao = '';
        this.os.checklist = [];
        this.os.fotoPreview = null;
        document.querySelector('input[type="file"]').value = '';


      } catch (error) {
        alert('Erro ao conectar com a API. Verifique se o terminal com "dotnet run" está aberto.');
      }
    }
  }
}).mount('#app')
