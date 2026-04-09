using System;
using System.Drawing;
using System.Windows.Forms;

namespace BurgerKiosk
{
    public partial class Form1 : Form
    {
        // 컨트롤 그룹화(탭/방향키 이동에 사용)
        private Control[] mainMenuControls;
        private Control[] optionControls;
        private Control[] orderControls;
        private Control[][] groups;

        // 탭 순서 제어용
        private Control[] tabOrder;

        public Form1()
        {
            InitializeComponent();

            // 폼에서 키 이벤트를 먼저 받도록 설정
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            this.Shown += Form1_Shown;

            // TabStop 보장 (Designer의 TabIndex는 그대로 유지되므로 건드리지 않음)
            rdoHamBurger.TabStop = true;
            rdoBulgogiBurger.TabStop = true;
            rdoChickenBurger.TabStop = true;

            chkPotato.TabStop = true;
            chkCola.TabStop = true;
            chkCheese.TabStop = true;
            chkSauce.TabStop = true;

            btnOrder.TabStop = true;
            btnRemove.TabStop = true;

            // 그룹 배열(방향키 이동에만 사용)
            mainMenuControls = new Control[] { rdoHamBurger, rdoBulgogiBurger, rdoChickenBurger };
            optionControls = new Control[] { chkPotato, chkCola, chkCheese, chkSauce };
            orderControls = new Control[] { btnOrder, btnRemove };
            groups = new Control[][] { mainMenuControls, optionControls, orderControls };

            // 탭 순서(요청): 햄버거, 불고기, 치킨, 감자튀김, 콜라, 치즈, 소스, 주문하기, 초기화
            tabOrder = new Control[]
            {
                rdoHamBurger,
                rdoBulgogiBurger,
                rdoChickenBurger,
                chkPotato,
                chkCola,
                chkCheese,
                chkSauce,
                btnOrder,
                btnRemove
            };

            // TabStop 보장 (안전)
            foreach (var c in tabOrder) if (c != null) c.TabStop = true;

            // 선택 변경 시 즉시 미리보기/총액 갱신 이벤트 연결
            rdoHamBurger.CheckedChanged += SelectionChanged;
            rdoBulgogiBurger.CheckedChanged += SelectionChanged;
            rdoChickenBurger.CheckedChanged += SelectionChanged;

            chkPotato.CheckedChanged += SelectionChanged;
            chkCola.CheckedChanged += SelectionChanged;
            chkCheese.CheckedChanged += SelectionChanged;
            chkSauce.CheckedChanged += SelectionChanged;

            // 초기 상태
            totalCost = 0;
            lblTotalCost.ForeColor = Color.Blue;
            lblTotalCost.Text = $"총 금액: {totalCost:N0}원";
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            // 실행 시 아무것도 선택되지 않도록 명시적으로 해제
            rdoHamBurger.Checked = false;
            rdoBulgogiBurger.Checked = false;
            rdoChickenBurger.Checked = false;
            chkPotato.Checked = false;
            chkCola.Checked = false;
            chkCheese.Checked = false;
            chkSauce.Checked = false;

            // 초기 포커스: 주문 버튼 (탭 시작점)
            btnOrder.Focus();
        }

        int totalCost = 0;

        // 공통: 라디오/체크박스 변경시 호출
        private void SelectionChanged(object? sender, EventArgs e)
        {
            UpdatePreview();
        }

        // 현재 선택 상태를 기준으로 lstOrder와 lblTotalCost를 즉시 갱신
        private void UpdatePreview()
        {
            // 메인 메뉴가 선택되지 않았으면 리스트 비우고 금액 0으로 표시
            if (!(rdoHamBurger.Checked || rdoBulgogiBurger.Checked || rdoChickenBurger.Checked))
            {
                lstOrder.Items.Clear();
                totalCost = 0;
                lblTotalCost.ForeColor = Color.Blue;
                lblTotalCost.Text = $"총 금액: {totalCost:N0}원";
                return;
            }

            lstOrder.Items.Clear();
            int sum = 0;

            if (rdoHamBurger.Checked)
            {
                sum += 5000;
                lstOrder.Items.Add("햄버거 5,000원");
            }
            else if (rdoBulgogiBurger.Checked)
            {
                sum += 4000;
                lstOrder.Items.Add("불고기버거 4,000원");
            }
            else if (rdoChickenBurger.Checked)
            {
                sum += 3000;
                lstOrder.Items.Add("치킨버거 3,000원");
            }

            if (chkPotato.Checked)
            {
                sum += 3500;
                lstOrder.Items.Add("감자 튀김 3,500원");
            }
            if (chkCola.Checked)
            {
                sum += 2500;
                lstOrder.Items.Add("콜라 2,500원");
            }
            if (chkCheese.Checked)
            {
                sum += 1500;
                lstOrder.Items.Add("치즈 추가 1,500원");
            }
            if (chkSauce.Checked)
            {
                sum += 500;
                lstOrder.Items.Add("소스 추가 500원");
            }

            totalCost = sum;
            lblTotalCost.ForeColor = Color.Blue;
            lblTotalCost.Text = $"총 금액: {sum:N0}원";
        }

        // ——— 기존 키 처리 로직 (간략) ———
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            // Tab: 사용자 정의 탭 순서로 이동 (Shift+Tab 역순)
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                int dir = (e.Modifiers & Keys.Shift) == Keys.Shift ? -1 : 1;
                MoveFocusInTabOrder(dir);
                return;
            }

            // 방향키: 현재 포커스된 그룹 내에서 포커스 이동
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
            {
                e.Handled = true;
                MoveFocusInCurrentGroup(-1);
                return;
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
            {
                e.Handled = true;
                MoveFocusInCurrentGroup(+1);
                return;
            }

            // Space: 포커스된 항목 선택(토글)
            if (e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                ActivateFocusedSelectable();
                return;
            }

            // Enter: 버튼 클릭 또는 현재 포커스된 항목 선택
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                var focused = GetFocusedControl();
                if (focused is Button btn)
                {
                    btn.PerformClick();
                    return;
                }
                ActivateFocusedSelectable();
                return;
            }
        }

        private Control? GetFocusedControl()
        {
            Control? ctl = this.ActiveControl;
            while (ctl is ContainerControl cc && cc.ActiveControl != null)
            {
                ctl = cc.ActiveControl;
            }
            return ctl;
        }

        private void MoveFocusInTabOrder(int direction)
        {
            if (tabOrder == null || tabOrder.Length == 0) return;

            var focused = GetFocusedControl();
            int idx = Array.IndexOf(tabOrder, focused);

            int startIdx = idx;
            if (startIdx < 0) startIdx = direction > 0 ? -1 : 0;

            int len = tabOrder.Length;
            for (int attempt = 0; attempt < len; attempt++)
            {
                int candidate = ((startIdx + direction * (attempt + 1)) % len + len) % len;
                var ctrl = tabOrder[candidate];
                if (ctrl != null && ctrl.Visible && ctrl.Enabled && ctrl.TabStop)
                {
                    ctrl.Focus();
                    return;
                }
            }
        }

        private void MoveFocusInCurrentGroup(int direction)
        {
            var focused = GetFocusedControl();
            foreach (var group in groups)
            {
                int idx = IndexOfControl(group, focused);
                if (idx >= 0)
                {
                    int newIndex = (idx + direction) % group.Length;
                    if (newIndex < 0) newIndex += group.Length;
                    group[newIndex].Focus();
                    return;
                }
            }
        }

        private void ActivateFocusedSelectable()
        {
            var focused = GetFocusedControl();
            if (focused is RadioButton rb) rb.Checked = true;
            else if (focused is CheckBox cb) cb.Checked = !cb.Checked;
            else if (focused is Button btn) btn.PerformClick();
        }

        private int IndexOfControl(Control[] arr, Control? target)
        {
            if (target == null) return -1;
            for (int i = 0; i < arr.Length; i++) if (arr[i] == target) return i;
            return -1;
        }
        // ——— 키 처리 로직 끝 ———

        private void btnOrder_Click(object sender, EventArgs e)
        {
            // 주문 버튼은 현재 상태를 다시 검증하고, 없으면 경고(기존 동작 유지)
            if (!(rdoHamBurger.Checked || rdoBulgogiBurger.Checked || rdoChickenBurger.Checked))
            {
                lblTotalCost.ForeColor = Color.Red;
                lblTotalCost.Text = "메뉴를 선택하세요.";
                mainMenuControls[0].Focus();
                return;
            }
    
            // 이미 SelectionChanged에서 미리보기가 갱신되므로, 추가 작업이 필요없음.
            // 필요하면 여기서 실제 주문 확정 로직(저장, DB 전송 등)을 추가합니다.
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

            totalCost = 0;
            lblTotalCost.ForeColor = Color.Blue;
            lblTotalCost.Text = $"총 금액: {totalCost:N0}원";

            btnOrder.Focus();
        }
    }
}
