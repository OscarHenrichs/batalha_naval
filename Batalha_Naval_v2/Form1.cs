﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Batalha_Naval_v2
{
    public partial class janela_jogo : Form
    {
        //****VARIAVEIS E ARRAYS UTILIZADOS****COMEÇO
        //
        //1)Labels e Botões gerados dentro dos paineis.
        //--> Jogador 1
        Label[,] quadrados_do_jogador_1 = new Label[10, 10]; // Armazenei cada label gerada dentro de uma matriz.
        Label[] valores_botoes_1 = new Label[4]; // Armazenei cada label gerada dentro de uma matriz.
        Button[] botoes_navios_1 = new Button[5];// Botões gerados com cada Navio.

        //--> Jogador 2
        Label[,] quadrados_do_jogador_2 = new Label[10, 10]; // Armazenei cada label gerada dentro de uma matriz.
        Label[] valores_botoes_2 = new Label[4]; // Armazenei cada label gerada dentro de uma matriz.
        Button[] botoes_navios_2 = new Button[5];// Botões gerados com cada Navio.

        //2) Constantes, Textos e imagens.
        const int n_linhas = 10;
        const int n_colunas = 10;
        const int tamanho_quadrados = 40;
        string[] QuantidadeVidasNavios = new string[4] { "5", "4", "3", "2" };//Mensagem de Texto dos Botões.
        Image[] imagem_do_botao = new Image[4] { Batalha_Naval_v2.Properties.Resources.navio5,
                                                 Batalha_Naval_v2.Properties.Resources.navio4,
                                                 Batalha_Naval_v2.Properties.Resources.navio3,
                                                 Batalha_Naval_v2.Properties.Resources.navio2 };// dentro de cada i está armazenado um direcionamento a imagem.

        //3) Valores Armazenados e reutilizados dentro das funções.
        //*** Jogador 1 ***
        //----> Booelans
        bool[,] quadrado_marcado_jogador_1 = new bool[10, 10]; // Um Boolean pra salvar os quadrados marcados.
        bool botao_ativo_tres_boolean_1 = false;// Serve para saber se é o primeiro barco de 3 ou o segundo barco de 3.
        //----> Arrays
        string[] txt_quantidade_de_navios = new string[4] { "1", "1", "2", "1" };//Mensagem de Texto dos Botões.
        int[] botao_ativo_1 = new int[4] { 1, 1, 1, 1 };
        int[] quantidade_posicoes_navios = new int[4] { 5, 4, 3, 2 };//Valor que cada Botão gerado representa pra posições dos navios.

        //*** Jogador 2 ***
        //----> Booleans
        bool[,] quadrado_marcado_jogador_2 = new bool[10, 10]; // Um Boolean pra salvar os quadrados marcados.
        bool botao_ativo_tres_boolean_2 = false;
        //----> Arrays
        int[] botao_ativo_2 = new int[4] { 1, 1, 1, 1 };

        //*** Apenas para armazenamento ***
        int Botao_Navio_Selecionado = 0;//Recebe o valor do botão selecionado.
        int posicao_do_navio_X = 0;
        int posicao_do_navio_Y = 0;
        bool direcao_navio_X = false;
        bool direcao_navio_Y = false;
        int fase = 1; //  <-------- ESTE VALOR DETERMINA A FASE EM QUE O JOGO ESTÁ (1 - DEF POS JG 1 || 2 - DEF POS JOG 2 || 3 - JOGANDO)
        int vez = 1;
        int pontos_j1 = 0;
        int pontos_j2 = 0;

        //****VARIAVEIS E ARRAYS UTILIZADOS****FIM


        //****INICIALIZAÇÃO DO JOGO****
        public janela_jogo()
        {
            InitializeComponent();
        }

        private void Janela_jogo_Load(object sender, EventArgs e)
        {
            definir_posicao_j1(); // <---- Na função de carregamento do jogo, é carregada esta função, de preparar para o jogador 1 selecionar seus navios.
        }
        //****INICIALIZAÇÃO DO JOGO****FIM

        private void B_pronto_Click(object sender, EventArgs e)
        {
            // fase é uma váriável para verificação da etapa do jogo.
            // Na condição abaixo, se a fase for 1 (que é a fase inicial do jogo) estiver settada e os navios estiverem colocados, clicando em "PRONTO" vai para a fase 2
            if (fase == 1 & (botao_ativo_1[0] == 6 & botao_ativo_1[1] == 5 & botao_ativo_1[2] == 4 & botao_ativo_1[3] == 3 & botao_ativo_tres_boolean_1 == true))
            {
                for (int i = 0; i < n_linhas; i++)
                {
                    for (int j = 0; j < n_colunas; j++)
                    {
                        quadrados_do_jogador_1[i, j].BackColor = System.Drawing.Color.Aqua;
                    }
                }
                fase = 2;
                definir_posicao_j2();
                status_jogo.Text = "Definindo jogo: Jogador 2";
            }

            else if (fase == 2 & (botao_ativo_2[0] == 6 & botao_ativo_2[1] == 5 & botao_ativo_2[2] == 4 & botao_ativo_2[3] == 3 & botao_ativo_tres_boolean_2 == true))
            {
                for (int i = 0; i < n_linhas; i++)
                {
                    for (int j = 0; j < n_colunas; j++)
                    {
                        quadrados_do_jogador_2[i, j].BackColor = System.Drawing.Color.Aqua;
                    }
                }
                fase = 3;
                Atirar(); //Esta função seria o "jogando".
                status_jogo.Text = "Vez do jogador 1";
            }
        }

        public void Atirar() // Esta função torna todos os quadrados do jogador 1 clicáveis através da função "Tiro_j2" e vice versa.
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    quadrados_do_jogador_1[i, j].Click += Tiro_j2;
                    quadrados_do_jogador_2[i, j].Click += Tiro_j1;
                }
            }
        }

        public void Tiro_j1(object sender, System.EventArgs e)
        {
            Label lbl = (Label)sender; // Referencia a label clicada como objeto "lbl"

            int retorna_cordenada_i = lbl.Location.X / tamanho_quadrados; //Retorna o valor em X simplificado
            int retorna_cordenada_j = lbl.Location.Y / tamanho_quadrados; //Retorna o valor em Y simplificado

            if (vez == 1) // Apenas se for a vez do jogador 1 a função faz alguma coisa.
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] == true)
                {
                    lbl.BackColor = System.Drawing.Color.Red;
                    pontos_j1++;
                    if (pontos_j1 == 17)
                    {
                        MessageBox.Show("Jogador 1 venceu!");
                    }
                    vez = 2;
                    status_jogo.Text = "Vez do jogador 2";
                }
                else if (lbl.BackColor == System.Drawing.Color.Aqua)
                {
                    lbl.BackColor = System.Drawing.Color.Yellow;
                    vez = 2;
                    status_jogo.Text = "Vez do jogador 2";
                }
            }
        }

        public void Tiro_j2(object sender, System.EventArgs e)
        {
            Label lbl = (Label)sender;

            int retorna_cordenada_i = lbl.Location.X / tamanho_quadrados;
            int retorna_cordenada_j = lbl.Location.Y / tamanho_quadrados;

            if (vez == 2)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] == true)
                {
                    lbl.BackColor = System.Drawing.Color.Red;
                    pontos_j2++;
                    if (pontos_j2 == 17)
                    {
                        MessageBox.Show("Jogador 2 venceu!");
                    }
                    vez = 1;
                    status_jogo.Text = "Vez do jogador 1";


                }
                else if (lbl.BackColor == System.Drawing.Color.Aqua)
                {
                    lbl.BackColor = System.Drawing.Color.Yellow;
                    vez = 1;
                    status_jogo.Text = "Vez do jogador 1";
                }
            }
        }

        void definir_posicao_j1() // Apenas para simplificar o processo de iniciação.
        {
            gerar_labels_j1();
            gerar_botoes_barcos_j1();
        }

        void definir_posicao_j2()
        {
            gerar_labels_j2();
            gerar_botoes_barcos_j2();
        }

        private void gerar_labels_j1()
        {
            //Com esses laços é construido a tabela de labels do joador 
            for (int i = 0; i < n_linhas; i++)
            {
                for (int j = 0; j < n_colunas; j++)
                {
                    quadrados_do_jogador_1[i, j] = new Label
                    {
                        Size = new Size(tamanho_quadrados, tamanho_quadrados),
                        Location = new Point(i * tamanho_quadrados, j * tamanho_quadrados),
                        BackColor = Color.Aqua,
                        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                    };

                    quadrados_do_jogador_1[i, j].Click += selecionar_navios_j1;// Direciona o click no painel a função Selecionar_navios.
                    quadrado_marcado_jogador_1[i, j] = false;// Deixei como padrão para todas as Labels False, quando cliacado muda para true, identifica os quadrados marcados
                    p_campo_1.Controls.Add(quadrados_do_jogador_1[i, j]); // Adiciona o painel de labels do jogador 1 no Form 1 onde ficaram os navios do COM.
                }
            }
        }

        private void gerar_botoes_barcos_j1() // GERA OS BOTÕES DOS NAVIOS E O NÚMERO DE NAVIOS
        {
            for (int i = 0; i < 4; i++)
            {
                botoes_navios_1[i] = new Button()
                {
                    Location = new Point(0, i * 60),
                    BackgroundImage = imagem_do_botao[i],
                    Size = new System.Drawing.Size(196, 60),
                    TabIndex = 1,
                    Text = QuantidadeVidasNavios[i],
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    ForeColor = Color.Red,
                    UseVisualStyleBackColor = true
                };
                p_botoes_barcos_j1.Controls.Add(botoes_navios_1[i]);
                botoes_navios_1[i].Click += Botao_Navios;

                valores_botoes_1[i] = new Label()
                {
                    Size = new Size(25, 30),
                    Location = new Point(0, 20 + (i * 60)),
                    BackColor = Color.White,
                    Text = txt_quantidade_de_navios[i],
                    Font = new Font("Arial", 16, FontStyle.Bold)
                };
                p_num_barcos_j1.Controls.Add(valores_botoes_1[i]);
            }
        }

        private void gerar_labels_j2()
        {
            //Com esses laços é construido a tabela de labels do joador 2
            for (int i = 0; i < n_linhas; i++)//
            {
                for (int j = 0; j < n_colunas; j++)
                {
                    quadrados_do_jogador_2[i, j] = new Label
                    {
                        Size = new Size(tamanho_quadrados, tamanho_quadrados),
                        Location = new Point(i * tamanho_quadrados, j * tamanho_quadrados),
                        BackColor = Color.Aqua,
                        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
                    };

                    quadrados_do_jogador_2[i, j].Click += selecionar_navios_j2;// Direciona o click no painel a função Selecionar_navios.
                    quadrado_marcado_jogador_2[i, j] = false;// Deixei como padrão para todas as Labels False, quando cliacado muda para true, identifica os quadrados marcados
                    p_campo_2.Controls.Add(quadrados_do_jogador_2[i, j]); // Adiciona o painel de labels do jogador 1 no Form 1 onde ficaram os navios do COM.
                }
            }
        }

        private void gerar_botoes_barcos_j2()
        {
            for (int i = 0; i < 4; i++)
            {
                botoes_navios_2[i] = new Button()
                {
                    Location = new Point(0, i * 60),
                    BackgroundImage = imagem_do_botao[i],
                    Size = new System.Drawing.Size(196, 60),
                    TabIndex = 1,
                    Text = QuantidadeVidasNavios[i],
                    Font = new Font("Arial", 16, FontStyle.Bold),
                    ForeColor = Color.Red,
                    UseVisualStyleBackColor = true
                };
                p_botoes_barcos_j2.Controls.Add(botoes_navios_2[i]);
                botoes_navios_2[i].Click += Botao_Navios;

                valores_botoes_2[i] = new Label()
                {
                    Name = $"(label {i})",
                    Size = new Size(25, 30),
                    Location = new Point(0, 20 + (i * 60)),
                    BackColor = Color.White,
                    Text = txt_quantidade_de_navios[i],
                    Font = new Font("Arial", 16, FontStyle.Bold)
                };
                p_num_barcos_j2.Controls.Add(valores_botoes_2[i]);
            }
        }

        private void selecionar_navios_j1(object sender, System.EventArgs e)//Aqui é da onde são tirados os valores de cada quadrado da tabala do jogo. 
        {
            //Pelo botão selecionado ele escolhe qual if seguir, falta limitar as direções possiveis.
            Label lbl = (Label)sender;//Ele armazena o caminho para o objeto label clicado

            //Funções reutilizadas dentro da função.
            bool validador_para_X = ((posicao_do_navio_X + tamanho_quadrados == lbl.Location.X || posicao_do_navio_X - tamanho_quadrados == lbl.Location.X) & posicao_do_navio_Y == lbl.Location.Y);
            bool validador_para_Y = ((posicao_do_navio_Y + tamanho_quadrados == lbl.Location.Y || posicao_do_navio_Y - tamanho_quadrados == lbl.Location.Y) & posicao_do_navio_X == lbl.Location.X);
            int retorna_cordenada_i = lbl.Location.X / tamanho_quadrados;
            int retorna_cordenada_j = lbl.Location.Y / tamanho_quadrados;

            //***Botão Jogador 1*** COMEÇO
                //---> Botão Navio de 5 posições - Começo
            if (Botao_Navio_Selecionado == 5)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_1[0] <= 5)// ok
                {
                    if (botao_ativo_1[0] == 1)//Primeiro Click
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[0] += 1;
                        posicao_do_navio_X = lbl.Location.X;//Salva a posição do navio para comparação com a nova posição
                        posicao_do_navio_Y = lbl.Location.Y;
                    }
                    else if (botao_ativo_1[0] == 2 & (validador_para_X || validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[0] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;
                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {//Segue para a Horizontal
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[0] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_1[0] == 6)
                        {
                            valores_botoes_1[0].Text = "0";
                            direcao_navio_X = false;
                        }
                    }
                    else if (direcao_navio_Y == true & validador_para_Y)
                    {//Segue para a vertical
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[0] += 1;
                        posicao_do_navio_Y = lbl.Location.Y;
                        if (botao_ativo_1[0] == 6)
                        {
                            valores_botoes_1[0].Text = "0";
                            direcao_navio_Y = false;
                        }
                    }
                }
                //---> Botão Navio de 5 posições - FIM
            }
            //---> Botão Navio de 4 posições - Começo
            else if (Botao_Navio_Selecionado == 4)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_1[1] <= 4)//ok
                {
                    if (botao_ativo_1[1] == 1)//Primeiro Click
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[1] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                    }
                    else if (botao_ativo_1[1] == 2 & (validador_para_X ^ validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[1] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;

                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {//Segue para a Horizontal
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[1] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_1[1] == 5)
                        {
                            valores_botoes_1[1].Text = "0";
                            direcao_navio_X = false;
                        }
                    }
                    else if (direcao_navio_Y == true & validador_para_Y)
                    {//Segue para a vertical
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[1] += 1;
                        posicao_do_navio_Y = lbl.Location.Y;
                        if (botao_ativo_1[1] == 5)
                        {
                            valores_botoes_1[1].Text = "0";
                            direcao_navio_Y = false;
                        }
                    }
                }
                //---> Botão Navio de 4 posições - FIM
            }
            //---> Botão Navio de 3 posições - Começo
            else if (Botao_Navio_Selecionado == 3)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_1[2] <= 3 & botao_ativo_tres_boolean_1 == false)
                {
                    if (botao_ativo_1[2] == 1)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                        Console.WriteLine(posicao_do_navio_X);
                    }
                    else if (botao_ativo_1[2] == 2 & (validador_para_X ^ validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;

                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_1[2] == 4)
                        {
                            botao_ativo_tres_boolean_1 = true;
                            botao_ativo_1[2] = 1;
                            valores_botoes_1[2].Text = "1";
                            direcao_navio_X = false;
                        }
                    }
                    else if (direcao_navio_Y & validador_para_Y)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        posicao_do_navio_X = lbl.Location.Y;
                        if (botao_ativo_1[2] == 4)
                        {
                            botao_ativo_tres_boolean_1 = true;
                            botao_ativo_1[2] = 1;
                            valores_botoes_1[2].Text = "1";
                            direcao_navio_Y = false;
                        }
                    }
                }
                else if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_1[2] <= 3 & botao_ativo_tres_boolean_1 == true)
                {
                    if (botao_ativo_1[2] == 1)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                        Console.WriteLine(posicao_do_navio_X);
                    }
                    else if (botao_ativo_1[2] == 2 & (validador_para_X ^ validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;

                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_1[2] == 4)
                        {
                            valores_botoes_1[2].Text = "0";
                            direcao_navio_X = false;

                        }
                    }
                    else if (direcao_navio_Y == true & validador_para_Y)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[2] += 1;
                        posicao_do_navio_Y = lbl.Location.Y;
                        if (botao_ativo_1[2] == 4)
                        {
                            valores_botoes_1[2].Text = "0";
                            direcao_navio_Y = false;
                        }
                    }
                }
                //---> Botão Navio de 3 posições - FIM
            }
            //---> Botão Navio de 2 posições - Começo
            else if (Botao_Navio_Selecionado == 2)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_1[3] <= 2)
                {
                    if (botao_ativo_1[3] == 1)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[3] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                        Console.WriteLine(posicao_do_navio_X);
                    }
                    else if (validador_para_X ^ validador_para_Y)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_1[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_1[3] += 1;
                        if (botao_ativo_1[3] == 3)
                        {
                            valores_botoes_1[3].Text = "0";
                        }
                    }
                }
                //---> Botão Navio de 2 posições - FIM
            }
        }

        private void selecionar_navios_j2(object sender, System.EventArgs e)
        {

            //Pelo botão selecionado ele escolhe qual if seguir, falta limitar as direções possiveis.
            Label lbl = (Label)sender;//Ele armazena o caminho para o objeto label clicado

            //Funções reutilizadas dentro da função.
            bool validador_para_X = ((posicao_do_navio_X + tamanho_quadrados == lbl.Location.X || posicao_do_navio_X - tamanho_quadrados == lbl.Location.X) & posicao_do_navio_Y == lbl.Location.Y);
            bool validador_para_Y = ((posicao_do_navio_Y + tamanho_quadrados == lbl.Location.Y || posicao_do_navio_Y - tamanho_quadrados == lbl.Location.Y) & posicao_do_navio_X == lbl.Location.X);
            int retorna_cordenada_i = lbl.Location.X / tamanho_quadrados;
            int retorna_cordenada_j = lbl.Location.Y / tamanho_quadrados;



            //---> Botão Navio de 5 posições - Começo
            if (Botao_Navio_Selecionado == 5)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_2[0] <= 5)// ok
                {
                    if (botao_ativo_2[0] == 1)//Primeiro Click
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[0] += 1;
                        posicao_do_navio_X = lbl.Location.X;//Salva a posição do navio para comparação com a nova posição
                        posicao_do_navio_Y = lbl.Location.Y;
                    }
                    else if (botao_ativo_2[0] == 2 & (validador_para_X || validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[0] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;
                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {//Segue para a Horizontal
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[0] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_2[0] == 6)
                        {
                            valores_botoes_2[0].Text = "0";
                            direcao_navio_X = false;
                        }
                    }
                    else if (direcao_navio_Y == true & validador_para_Y)
                    {//Segue para a vertical
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[0] += 1;
                        posicao_do_navio_Y = lbl.Location.Y;
                        if (botao_ativo_2[0] == 6)
                        {
                            valores_botoes_2[0].Text = "0";
                            direcao_navio_X = false;
                        }
                    }
                }
                //---> Botão Navio de 5 posições - FIM
            }
            //---> Botão Navio de 4 posições - Começo
            else if (Botao_Navio_Selecionado == 4)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_2[1] <= 4)//ok
                {
                    if (botao_ativo_2[1] == 1)//Primeiro Click
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[1] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                    }
                    else if (botao_ativo_2[1] == 2 & (validador_para_X ^ validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[1] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;

                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {//Segue para a Horizontal
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[1] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_2[1] == 5)
                        {
                            valores_botoes_2[1].Text = "0";
                            direcao_navio_X = false;
                        }
                    }
                    else if (direcao_navio_Y == true & validador_para_Y)
                    {//Segue para a vertical
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[1] += 1;
                        posicao_do_navio_Y = lbl.Location.Y;
                        if (botao_ativo_2[1] == 5)
                        {
                            valores_botoes_2[1].Text = "0";
                            direcao_navio_Y = false;
                        }
                    }
                }//---> Botão Navio de 4 posições - FIM
            }
            //---> Botão Navio de 3 posições - Começo
            else if (Botao_Navio_Selecionado == 3)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_2[2] <= 3 & botao_ativo_tres_boolean_2 == false)
                {
                    if (botao_ativo_2[2] == 1)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                        Console.WriteLine(posicao_do_navio_X);
                    }
                    else if (botao_ativo_2[2] == 2 & (validador_para_X ^ validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;

                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_2[2] == 4)
                        {
                            botao_ativo_tres_boolean_2 = true;
                            botao_ativo_2[2] = 1;
                            valores_botoes_2[2].Text = "1";
                            direcao_navio_X = false;
                        }
                    }
                    else if (direcao_navio_Y & validador_para_Y)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        posicao_do_navio_X = lbl.Location.Y;
                        if (botao_ativo_2[2] == 4)
                        {
                            botao_ativo_tres_boolean_2 = true;
                            botao_ativo_2[2] = 1;
                            valores_botoes_2[2].Text = "1";
                            direcao_navio_Y = false;
                        }
                    }
                }
                else if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_2[2] <= 3 & botao_ativo_tres_boolean_2 == true)
                {
                    if (botao_ativo_2[2] == 1)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                        Console.WriteLine(posicao_do_navio_X);
                    }
                    else if (botao_ativo_2[2] == 2 & (validador_para_X ^ validador_para_Y))
                    {//Define a direçao do navio
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        if (validador_para_X)
                        {
                            direcao_navio_X = true;//Definiu a direção do navio Y
                            posicao_do_navio_X = lbl.Location.X;
                        }
                        else if (validador_para_Y)
                        {
                            direcao_navio_Y = true;//Definiu a direção do navio em Y
                            posicao_do_navio_Y = lbl.Location.Y;
                        }
                    }
                    else if (direcao_navio_X == true & validador_para_X)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        if (botao_ativo_2[2] == 4)
                        {
                            valores_botoes_2[2].Text = "0";
                            direcao_navio_X = false;

                        }
                    }
                    else if (direcao_navio_Y == true & validador_para_Y)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[2] += 1;
                        posicao_do_navio_Y = lbl.Location.Y;
                        if (botao_ativo_2[2] == 4)
                        {
                            valores_botoes_2[2].Text = "0";
                            direcao_navio_Y = false;
                        }
                    }
                }
                //---> Botão Navio de 3 posições - FIM
            }
            //---> Botão Navio de 2 posições - Começo
            else if (Botao_Navio_Selecionado == 2)
            {
                if (lbl.BackColor == System.Drawing.Color.Aqua & botao_ativo_2[3] <= 2)
                {
                    if (botao_ativo_2[3] == 1)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[3] += 1;
                        posicao_do_navio_X = lbl.Location.X;
                        posicao_do_navio_Y = lbl.Location.Y;
                        Console.WriteLine(posicao_do_navio_X);
                    }
                    else if (validador_para_X ^ validador_para_Y)
                    {
                        lbl.BackColor = System.Drawing.Color.Black;
                        quadrado_marcado_jogador_2[retorna_cordenada_i, retorna_cordenada_j] = true; // Pela localização X e Y divididos pelo tamanho do quadrado chego no i e j.
                        botao_ativo_2[3] += 1;
                        if (botao_ativo_2[3] == 3)
                        {
                            valores_botoes_2[3].Text = "0";
                        }
                    }
                }//---> Botão Navio de 2 posições - FIM
            }
        }// *** Botão Jogador 2 *** FIM

        private void Botao_Navios(object sender, EventArgs e)
        {
            Button clicarBotao = (Button)sender;
            Botao_Navio_Selecionado = quantidade_posicoes_navios[clicarBotao.Location.Y / 60];
        }
    }
}