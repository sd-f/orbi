package foundation.softwaredesign.orbi.service.auth;

import foundation.softwaredesign.orbi.model.auth.AuthorizationInfo;
import foundation.softwaredesign.orbi.model.auth.LoginInfo;
import foundation.softwaredesign.orbi.model.auth.RequestCodeInfo;
import foundation.softwaredesign.orbi.model.game.character.Character;
import foundation.softwaredesign.orbi.persistence.entity.IdentityEntity;
import foundation.softwaredesign.orbi.persistence.repo.auth.IdentityRepository;
import foundation.softwaredesign.orbi.persistence.types.ChkPass;
import foundation.softwaredesign.orbi.service.auth.IdentityThreadLocal;
import foundation.softwaredesign.orbi.service.game.character.CharacterService;
import org.apache.commons.lang3.RandomStringUtils;

import javax.annotation.PostConstruct;
import javax.annotation.Resource;
import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.mail.Address;
import javax.mail.Message;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.AddressException;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import javax.persistence.NoResultException;
import javax.validation.Valid;
import javax.validation.constraints.NotNull;
import javax.ws.rs.InternalServerErrorException;
import java.util.Date;
import java.util.Objects;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class UserService {

    private AuthorizationInfo authorizationInfo = new AuthorizationInfo();
    private IdentityEntity identityEntity;

    @Resource(mappedName = "java:comp/env/SDFMail")
    private Session smtpSession;

    @Inject
    IdentityRepository identityRepository;
    @Inject
    CharacterService characterService;

    @PostConstruct
    public void init() {
        this.identityEntity = IdentityThreadLocal.get();
    }

    public IdentityEntity getIdentity() {
        return this.identityEntity;
    }

    public AuthorizationInfo getAuthorizationInfo() {
        return this.authorizationInfo;
    }


    public void requestPassword(@Valid @NotNull RequestCodeInfo requestCodeInfo) {
        if (!isValidEmailAddress(requestCodeInfo.getEmail())) {
            throw new InternalServerErrorException("Email not valid");
        }
        IdentityEntity identityEntity = identityRepository.findByEmail(requestCodeInfo.getEmail());
        Boolean isNewIdentity = false;
        if (Objects.isNull(identityEntity)) {
            identityEntity = new IdentityEntity();
            identityEntity.setEmail(requestCodeInfo.getEmail());
            isNewIdentity = true;
        }
        identityEntity.setLastInit(new Date());

        String password = RandomStringUtils.randomAlphanumeric(20).toUpperCase();
        identityEntity.setTmpPassword(new ChkPass(password));

        saveUser(identityEntity);
        if (isNewIdentity) {
            Character newCharacter = characterService.create(identityEntity.getId());
            characterService.save(newCharacter);
        }
        if (!sendPasswordMail(identityEntity.getEmail(), password)) {
            throw new InternalServerErrorException("Email could not be send");
        }
    }

    public AuthorizationInfo login(@Valid @NotNull LoginInfo loginInfo) {
        if (!isValidEmailAddress(loginInfo.getEmail())) {
            throw new InternalServerErrorException("Email not valid");
        }
        IdentityEntity identityEntity = identityRepository.findByEmail(loginInfo.getEmail());
        if (Objects.isNull(identityEntity)) {
            throw new InternalServerErrorException("Email not registered");
        }
        try {
            Long id = identityRepository.findIdentityIdByEmailAndPassword(identityEntity.getEmail(), loginInfo.getPassword().toUpperCase());
            identityEntity = identityRepository.findBy(id);
        } catch (NoResultException ex) {
            throw new InternalServerErrorException("Email or Password incorrect");
        }

        identityEntity.setLastInit(new Date());

        String token = RandomStringUtils.randomAlphanumeric(100);
        identityEntity.setToken(new ChkPass(token));

        AuthorizationInfo authorizationInfo = new AuthorizationInfo();
        authorizationInfo.setToken(token);

        saveUser(identityEntity);
        return authorizationInfo;
    }

    private void saveUser(IdentityEntity identity) {
        identityRepository.saveAndFlushAndRefresh(identity);
    }

    private Boolean sendPasswordMail(String email, String password) {

        final Message message = new MimeMessage(smtpSession);
        try {
            message.setRecipients(Message.RecipientType.TO, new Address[]{
                    new InternetAddress(email)
            });
            message.setFrom(new InternetAddress("orbi@softwaredesign.foundation"));
            message.setSubject("Code for Orbi");
            message.setSentDate(new Date());
            message.setText("This is your code: " + password);
            Transport.send(message);
        } catch (Exception e) {
            Logger.getLogger(this.getClass().getName()).log(Level.SEVERE, null, e);
            return false;
        }
        return true;
    }

    public static boolean isValidEmailAddress(String email) {
        boolean result = true;
        try {
            InternetAddress emailAddr = new InternetAddress(email);
            emailAddr.validate();
        } catch (AddressException ex) {
            result = false;
        }
        return result;
    }

    public void updateLastInit() {
        identityEntity.setLastInit(new Date());
        saveUser(identityEntity);
    }
}
