using System;
using System.Drawing;
using System.Windows.Forms;

namespace BurgerKiosk
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // 앱 시작 시 라디오/체크박스가 자동으로 체크되지 않도록 명시적으로 해제
            rdoHamBurger.Checked = false;
            rdoBulgogiBurger.Checked = false;
            rdoChickenBurger.Checked = false;
            chkPotato.Checked = false;
            chkCola.Checked = false;
            chkCheese.Checked = false;
            chkSauce.Checked = false;

            // 총액 초기화 및 라벨 기본 표시(파란색 유지)
            totalCost = 0;
            lblTotalCost.ForeColor = Color.Blue;
            lblTotalCost.Text = $"총 금액: {totalCost:N0}원";
        }

        int totalCost = 0;
        private void btnOrder_Click(object sender, EventArgs e)
        {
            // 메인 메뉴(라디오) 하나도 선택되지 않았을 경우 메시지 표시 (빨간색) 후 종료
            bool hasMainMenu = rdoHamBurger.Checked || rdoBulgogiBurger.Checked || rdoChickenBurger.Checked;
            if (!hasMainMenu)
            {
                lblTotalCost.ForeColor = Color.Red;
                lblTotalCost.Text = "메뉴를 선택하세요.";
                return;
            }

            if (rdoHamBurger.Checked)
            {
                totalCost += 5000;
                lstOrder.Items.Add("햄버거 5,000원");
            }
            else if (rdoBulgogiBurger.Checked)
            {
                totalCost += 4000;
                lstOrder.Items.Add("불고기버거 4,000원");
            }
            else if (rdoChickenBurger.Checked)
            {
                totalCost += 3000;
                lstOrder.Items.Add("치킨버거 3,000원");
            }

            if (chkPotato.Checked)
            {
                totalCost += 3500;
                lstOrder.Items.Add("감자 튀김 3,500원");
            }
            if (chkCola.Checked)
            {
                totalCost += 2500;
                lstOrder.Items.Add("콜라 2,500원");
            }
            if (chkCheese.Checked)
            {
                totalCost += 1500;
                lstOrder.Items.Add("치즈 추가 1,500원");
            }
            if (chkSauce.Checked)
            {
                totalCost += 500;
                lstOrder.Items.Add("소스 추가 500원");
            }

            // 천 단위 구분 쉼표 적용 및 글자색 파란색 유지
            lblTotalCost.ForeColor = Color.Blue;
            lblTotalCost.Text = $"총 금액: {totalCost:N0}원";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            rdoHamBurger.Checked = false;
            rdoBulgogiBurger.Checked = false;
            rdoChickenBurger.Checked = false;
            chkPotato.Checked = false;
            chkCola.Checked = false;
            chkCheese.Checked = false;
            chkSauce.Checked = false;
            lstOrder.Items.Clear();

            // 총액 초기화 및 라벨 파란색으로 유지
            totalCost = 0;
            lblTotalCost.ForeColor = Color.Blue;
            lblTotalCost.Text = $"총 금액: {totalCost:N0}원";
        }
    }
}
