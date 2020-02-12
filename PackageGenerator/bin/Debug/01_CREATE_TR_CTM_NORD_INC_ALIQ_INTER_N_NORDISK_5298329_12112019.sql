CREATE OR REPLACE TRIGGER tr_ctm_nord_inc_aliq_inter
AFTER UPDATE ON NFE_NF_CAPA
REFERENCING NEW AS NEW OLD AS OLD
FOR EACH ROW
WHEN (NEW.STATUS_PROCESSAMENTO = 6)
/* **************************************************************************************************
  HISTORICO DE MODIFICACOES
  ----------------------------------------------------------------------------------------------------
  TECNICO                 DATA        CHAMADO     OBSERVACOES
  --------------- ----------- ----------- ------------------------------------------------------------
  ALLAN JHONY EFFTING   12/11/2019    5298329     CRIAÇÃO DE TRIGGER PARA ALTERACAO DO CAMPO ALIQUOTA.
  ************************************************************************************************** */

DECLARE 

	V_ALIQUOTA  NUMBER(15,4);

BEGIN
          
    FOR i IN (
        SELECT ITEM.ORIG_MERC,IMPOSTO.COD_IMPOSTO, IMPOSTO.MOT_DES_ICMS
        FROM NFE_NF_ITEM ITEM, NFE_NF_IMPOSTO IMPOSTO
        WHERE IMPOSTO.COD_IMPOSTO = '01' AND
			  ITEM.COD_HOLDING = IMPOSTO.COD_HOLDING AND
              ITEM.COD_MATRIZ = IMPOSTO.COD_MATRIZ AND
              ITEM.COD_FILIAL = IMPOSTO.COD_FILIAL AND
              ITEM.COD_INTERFACE = IMPOSTO.COD_INTERFACE AND
              ITEM.DOCNUM = IMPOSTO.DOCNUM AND
              ITEM.TIPO_NOTA = IMPOSTO.TIPO_NOTA AND
              ITEM.ID_ITEM = IMPOSTO.ID_ITEM AND
              ITEM.COD_HOLDING = :NEW.COD_HOLDING AND
              ITEM.COD_MATRIZ = :NEW.COD_MATRIZ AND
              ITEM.COD_FILIAL = :NEW.COD_FILIAL AND
              ITEM.COD_INTERFACE = :NEW.COD_INTERFACE AND
              ITEM.DOCNUM = :NEW.DOCNUM AND
              ITEM.TIPO_NOTA = :NEW.TIPO_NOTA AND
              IMPOSTO.MOT_DES_ICMS = 8 AND
              ITEM.ORIG_MERC IN ('1','2','3','4','5','6','7','8')
              ) LOOP  
                  IF :NEW.COD_UF IN ('MG','SP','RS','SC','RJ') AND  i.ORIG_MERC IN ('5','8') THEN
                      
                      V_ALIQUOTA := 12;
                      
                  END IF;
                  
                  IF :NEW.COD_UF NOT IN ('MG','SP','RS','SC','RJ') AND  i.ORIG_MERC IN ('5','8') THEN
                      
                      V_ALIQUOTA := 7;
                      
                  END IF;
                  
                  IF i.ORIG_MERC IN ('1','2','3','4','6','7') THEN
                      
                      V_ALIQUOTA := 4;
                      
                  END IF;
				  
                  UPDATE NFE_NF_IMPOSTO SET ALIQUOTA = V_ALIQUOTA
					  WHERE NFE_NF_IMPOSTO.COD_IMPOSTO = 'J6' OR 
							NFE_NF_IMPOSTO.COD_IMPOSTO = 'J7' AND
							NFE_NF_IMPOSTO.COD_HOLDING = :NEW.COD_HOLDING AND 
							NFE_NF_IMPOSTO.COD_MATRIZ = :NEW.COD_MATRIZ AND
							NFE_NF_IMPOSTO.COD_FILIAL = :NEW.COD_FILIAL AND
							NFE_NF_IMPOSTO.COD_INTERFACE = :NEW.COD_INTERFACE AND
							NFE_NF_IMPOSTO.DOCNUM = :NEW.DOCNUM AND
							NFE_NF_IMPOSTO.TIPO_NOTA = :NEW.TIPO_NOTA;
                
              END LOOP;
END;
/