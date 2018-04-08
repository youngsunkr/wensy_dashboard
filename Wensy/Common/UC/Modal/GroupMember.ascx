<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupMember.ascx.cs" Inherits="ServicePoint.Common.UC.Modal.GroupMember" %>
<script type="text/javascript">
    function AddMember() {

        var frm = $('#frmProc');
        var $container = $('#_AddMember');
        if (!emailCheck($container.find('#strEmail').val())) {
            return;
        }
        if (!$container.find('#strPass').val()) {
            $container.find('#strPass').focus();
            return;
        }
        if (!$container.find('#strPass_Confirm').val()) {
            $container.find('#strPass_Confirm').focus();
            return;
        } if (!$container.find('#strMemberName').val()) {
            $container.find('#strMemberName').focus();
            return;
        } if (!$container.find('#strTel').val()) {
            $container.find('#strTel').focus();
            return;
        } if ($container.find('#strPass').val() != $container.find('#strPass_Confirm').val()) {
            alert("비밀번호가 일치하지않습니다")
            $container.find('#strPass_Confirm').focus();
            return;
        }

        $('#command').val('addmember');
        $('#hdn_Email').val($container.find('#strEmail').val());
        $('#hdn_Pass').val($container.find('#strPass').val());
        $('#hdn_MemberName').val($container.find('#strMemberName').val());
        $('#hdn_Tel').val($container.find('#strTel').val());
        $('#hdn_Grade').val($container.find('#numGrade').val());


        var strBtnSubmit = $container.find('#btnSubmit').val();
        $container.find('#btnSubmit').val('Sending...').attr('disabled', 'disabled');


        $.getJSON('/Common/Proc/Admin.ashx?callback=?', $('#frmProc').serialize(), function (json) {
            if (1 == json.error) {
                alert(json.desc);
                $('#frm').submit();
            } else {
                alert(json.desc);
                $container.find('#btnSubmit').val(strBtnSubmit).removeAttr('disabled');
            }
        }).error(function (xhr) {
            alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
        });
    }
    function UpdateMember() {

        var frm = $('#frmProc');
        var $container = $('#_UpdateMember');
        if (!emailCheck($container.find('#strEmail').val())) {
            return;
        }
        
        $('#command').val('updatemember');
        $('#hdn_Email').val($container.find('#strEmail').val()); 
        $('#hdn_Grade').val($container.find('#numGrade').val());


        var strBtnSubmit = $container.find('#btnSubmit').val();
        $container.find('#btnSubmit').val('Sending...').attr('disabled', 'disabled');


        $.getJSON('/Common/Proc/Admin.ashx?callback=?', $('#frmProc').serialize(), function (json) {
            if (1 == json.error) {
                alert(json.desc);
                $('#frm').submit();
            } else {
                alert(json.desc);
                $container.find('#btnSubmit').val(strBtnSubmit).removeAttr('disabled');
            }
        }).error(function (xhr) {
            alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
        });
    }
    function DelMember(CompanyNum_Target) {

        var frm = $('#frmProc');
        if (confirm("삭제하시겠습니까?") != true)
        {
            return;
        }
        $('#command').val('delmember');
        $('#hdn_MemberNum_Target').val(CompanyNum_Target);

        $.getJSON('/Common/Proc/Admin.ashx?callback=?', $('#frmProc').serialize(), function (json) {
            if (1 == json.error) {
                alert(json.desc);
                $('#frm').submit();
            } else {
                alert(json.desc);
            }
        }).error(function (xhr) {
            alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
        });
    }
    function EmailCheck() {
        var $container = $('#_AddMember');
        //if (!$container.find('#strMemo').val()) {
        //    $container.find('#strMemo').focus();
        //    return;
        //}
        $('#command').val('emailcheck');
       
        if (!emailCheck($container.find('#strEmail').val())) {
            return;
        }
        else {
            $('#command').val('emailcheck');
            $('#hdn_Email').val($container.find('#strEmail').val());

            $.getJSON('/Common/Proc/Admin.ashx?callback=?', $('#frmProc').serialize(), function (json) {
                if (1 == json.error) {
                    alert(json.desc);
                    $container.find('#strEmail').attr('readonly', true);
                } else {
                    alert(json.desc);
                    $container.find('#strEmail').attr('readonly', false);
                }
            }).error(function (xhr) {
                alert('[' + xhr.status + '] Unexpected error has occurred.  Please try again.');
            });
            
        }
    }
    function modalAddMember() {

        var $frm = $('#frmProc');
        var $container = $('#_AddMember');
        $('#command').val('addmember');
        $container.find('#strEmail').val('');
        $container.find('#strPass').val('');
        $container.find('#strPass_Confirm').val('');
        $container.find('#strMemberName').val('');
        $container.find('#strTel').val('');
        $container.find('#strEmail').removeAttr('readonly');
        $container.find('#btnSubmit').removeAttr('disabled');
        //$frm.find('#hdn_numWorld').val(numWorld);
        //$frm.find('#hdn_oidAccount').val(oidAccount);
        //$frm.find('#numPoint_Mileage').val(numPoint_Mileage);
        //$container.find('#oidAccount').val(oidAccount).attr("readonly", true);;
        //$container.find('#numPoint_Mileage').val(numPoint_Mileage);

        $('#frmProc').submit(function (e) {
            e.preventDefault();
        });
        $container.modal();
    }
    function modalUpdateMember(strEmail,strMemberName,CompanyNum_Target,numGrade) {

        var $frm = $('#frmProc');
        var $container = $('#_UpdateMember');
        $('#command').val('updatemember');
        $('#hdn_MemberNum_Target').val(CompanyNum_Target); 
        $container.find('#strEmail').val(strEmail);
        $container.find('#strMemberName').val(strMemberName);
        $container.find('#numGrade').val(numGrade);
        $container.find('#strEmail').attr('readonly', true);
        $container.find('#strMemberName').attr('readonly', true);
        $container.find('#btnSubmit').removeAttr('disabled');
        //$frm.find('#hdn_numWorld').val(numWorld);
        //$frm.find('#hdn_oidAccount').val(oidAccount);
        //$frm.find('#numPoint_Mileage').val(numPoint_Mileage);
        //$container.find('#oidAccount').val(oidAccount).attr("readonly", true);;
        //$container.find('#numPoint_Mileage').val(numPoint_Mileage);

        $('#frmProc').submit(function (e) {
            e.preventDefault();
        });
        $container.modal();
    }
</script>
<form id="frmProc" name="frmProc" method="post" class="form-horizontal">
    <input type="hidden" id="command" name="command" />
    <input type="hidden" id="hdn_Email" name="hdn_Email" />
    <input type="hidden" id="hdn_Pass" name="hdn_Pass" />
    <input type="hidden" id="hdn_MemberName" name="hdn_MemberName" />
    <input type="hidden" id="hdn_Tel" name="hdn_Tel" />
    <input type="hidden" id="hdn_Grade" name="hdn_Grade" />
    <input type="hidden" id="hdn_MemberNum_Target" name="hdn_MemberNum_Target" />
</form>
<div class="modal fade form-horizontal" id="_AddMember">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header text-info">
                <a class="close" data-dismiss="modal">×</a>
                <h3>사용자 추가</h3>
            </div>
            <div class="modal-body">
                <fieldset>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strEmail">
                            E-Mail 계정
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strEmail" name="strEmail" /><input type="button" class="btn" id="EmailCheck" name="EmailCheck" value="EmailCheck" onclick="EmailCheck();" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strPass">
                            비밀번호
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="password" class="form-control" id="strPass" name="strPass" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strPass_Confirm">
                            비밀번호확인
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="password" class="form-control" id="strPass_Confirm" name="strPass_Confirm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strMemberName">
                            멤버이름
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strMemberName" name="strMemberName" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strTel">
                            전화번호
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strTel" name="strTel" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="numGrade">
                            멤버등급
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="numGrade" name="numGrade" class="form-control">
                                <option value="1">Operator</option>
                                <option value="8">Manager</option>
                                <option value="9">Owner</option>
                            </select>
                        </div>
                    </div>
                </fieldset>

            </div>
            <div class="modal-footer">
                <a data-dismiss="modal" class="btn btn-small btn-warning">닫기</a>
                <input type="button" id="btnSubmit" class="btn btn-small btn-primary" value="추가"
                    onclick="AddMember();" />
            </div>
        </div>
    </div>
</div>
<div class="modal fade form-horizontal" id="_UpdateMember">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header text-info">
                <a class="close" data-dismiss="modal">×</a>
                <h3>사용자 추가</h3>
            </div>
            <div class="modal-body">
                <fieldset>
                    <div class="form-group">
                        <label class="control-label col-sm-3 col-xs-3" for="strEmail">
                            E-Mail 계정
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strEmail" name="strEmail" />
                        </div>
                    </div><div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="strMemberName">
                            멤버이름
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <input type="text" class="form-control" id="strMemberName" name="strMemberName" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label  col-sm-3 col-xs-3" for="numGrade">
                            멤버등급
                        </label>
                        <div class="controls col-sm-9 col-xs-9">
                            <select id="numGrade" name="numGrade" class="form-control">
                                <option value="1">Operator</option>
                                <option value="8">Manager</option>
                                <option value="9">Owner</option>
                            </select>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="modal-footer">
                <a data-dismiss="modal" class="btn btn-small btn-warning">닫기</a>
                <input type="button" id="btnSubmit" class="btn btn-small btn-primary" value="수정"
                    onclick="UpdateMember();" />
            </div>
        </div>
    </div>
</div>
